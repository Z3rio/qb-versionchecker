#!/usr/bin/env node

import { input } from "@inquirer/prompts";
import { Dirent, existsSync, readdirSync } from "fs";
import path from "path";
import picocolors from "picocolors";

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

async function main() {
  const resourcesPath = await input({
    message: "Input resources folder path"
  });

  if (!existsSync(resourcesPath)) {
    console.warn(picocolors.red("Resources folder not found"));
    process.exit(1);
  }

  console.info(picocolors.green("Resources folder found"));
  console.info("\n");

  const qbDirectories = await getDirectories(resourcesPath);
  void qbDirectories;

  console.info("\n");
}

main();
