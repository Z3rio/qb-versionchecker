const concurrently = require('concurrently');
const path = require("path")
const picocolors = require("picocolors")

const argv = process.argv.slice(-1)
let commands = []

function success() {
    console.log("\n" + picocolors.greenBright('All commands ran successfully'));
    process.exit(0);
}

function failure() {
    console.error("\n" + picocolors.redBright('Some commands failed'));
    process.exit(1);
}

switch (argv[0]) {
    case "--check":
        commands = [
            {
                command: 'pnpm format:check',
                name: 'vue',
                prefixColor: 'green',
                cwd: path.resolve(__dirname, 'apps/vue'),
            },
            {
                command: 'pnpm format:check',
                name: 'backend',
                prefixColor: 'yellow',
                cwd: path.resolve(__dirname, 'apps/backend'),
            },
        ]
        break;
    case "--write":
        commands = [
            {
                command: 'pnpm format:write',
                name: 'vue',
                prefixColor: 'green',
                cwd: path.resolve(__dirname, 'apps/vue'),
            },
            {
                command: 'pnpm format:write',
                name: 'backend',
                prefixColor: 'yellow',
                cwd: path.resolve(__dirname, 'apps/backend'),
            },
        ]
        break;
}

const { result } = concurrently(commands);
result.then(success, failure);