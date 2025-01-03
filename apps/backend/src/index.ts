#!/usr/bin/env node

import { input } from "@inquirer/prompts";
import axios from "axios";
import { execSync } from "child_process";
import { Dirent, existsSync, readdirSync, renameSync, rmSync } from "fs";
import path from "path";
import picocolors from "picocolors";
import "dotenv/config";
import minimist from "minimist";

// Handle inquirer errors
process.on("uncaughtException", error => {
  if (!(error instanceof Error) || error.name !== "ExitPromptError") {
    throw error;
  }
});

function getDirectories(currentPath: string, list: Dirent[] = []): Dirent[] {
  const directories = readdirSync(currentPath, { withFileTypes: true });

  for (const directory of directories) {
    if (directory.isDirectory()) {
      if (directory.name.startsWith("qb-")) {
        list.push(directory);
      } else {
        list = getDirectories(path.join(currentPath, directory.name), list);
      }
    }
  }

  return list;
}

async function wait(ms: number) {
  await new Promise(resolve => {
    setTimeout(resolve, ms);
  });
}

async function main() {
  const argv = minimist(process.argv.slice(2));
  const isDev = argv.dev === true;
  const resourcesPath = !isDev
    ? await input({
        message: "Input resources folder path"
      })
    : (process.env.RESOURCES_PATH ?? "");

  if (!existsSync(resourcesPath)) {
    console.warn(picocolors.red("Resources folder not found"));
    process.exit(1);
  }

  console.info(picocolors.green("Resources folder found"));
  console.info("\n");

  const qbDirectories = getDirectories(resourcesPath);
  void qbDirectories;

  const validatedDirectories: Dirent[] = isDev ? qbDirectories : [];

  if (!isDev) {
    for (const directory of qbDirectories) {
      console.info(
        picocolors.blueBright(
          `Validating resource: ${picocolors.bold(directory.name)}`
        )
      );

      try {
        const response = await axios.get(
          `https://api.github.com/repos/qbcore-framework/${directory.name}`
        );

        if (response.status === 200) {
          validatedDirectories.push(directory);
        }
      } catch (err) {
        if (axios.isAxiosError(err) && err.response?.status === 403) {
          console.warn(
            picocolors.red("GitHub rate limit reached, please try again later")
          );
          process.exit(1);
        }
      }

      // dont run loop too fast
      await wait(100);
    }
  }

  console.info(picocolors.green("Validated resources"));
  console.info("\n");

  for (const directory of validatedDirectories) {
    try {
      const directoryPath = path.join(directory.path, directory.name);

      if (!existsSync(path.join(directoryPath, ".git"))) {
        console.info(
          picocolors.blueBright(
            `Setting up .git for repository ${picocolors.bold(directory.name)}`
          )
        );

        const tempGitPath = path.join(directoryPath, "tempGit");

        if (existsSync(tempGitPath)) {
          rmSync(tempGitPath, {
            recursive: true,
            force: true
          });
        }

        execSync(
          `git clone --no-checkout "https://github.com/qbcore-framework/${directory.name}" tempGit`,
          {
            cwd: directoryPath,
            stdio: "inherit"
          }
        );

        renameSync(
          path.join(tempGitPath, ".git"),
          path.join(directoryPath, ".git")
        );

        rmSync(tempGitPath, {
          recursive: true,
          force: true
        });
      } else {
        console.info(
          picocolors.greenBright(
            `.git already exists for repository ${picocolors.bold(directory.name)}`
          )
        );
      }

      const previousCommit = execSync("git rev-parse HEAD", {
        cwd: directoryPath
      }).toString();

      execSync("git stash", {
        cwd: directoryPath,
        stdio: "inherit"
      });
      execSync("git fetch --all", {
        cwd: directoryPath,
        stdio: "inherit"
      });
      execSync("git pull origin main", {
        cwd: directoryPath,
        stdio: "inherit"
      });

      const updatedCommit = execSync("git rev-parse HEAD", {
        cwd: directoryPath
      }).toString();

      if (previousCommit !== updatedCommit) {
        console.info(
          picocolors.greenBright(
            `Repository ${picocolors.bold(directory.name)} updated`
          )
        );
      }

      console.info("\n");
    } catch (err) {
      console.warn(`Couldnt handle repository ${directory.name}`);
      void err;
    }
  }

  console.info("\n");
}

main();
