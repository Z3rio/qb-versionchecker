#!/bin/bash

printf "Input resources folder: "
read resourceFolder

# Validate input folder path
if [[ ! -d "$resourceFolder" ]]; then
    printf "\nInvalid folder path\n"
    exit 1
fi

handleResource() {
    local resourcePath="$1"
    local name
    name="$(basename "$resourcePath")"
    printf "Handling resource: %s\n" "$name"

    # Check if the GitHub repository exists
    local responseCode
    responseCode=$(curl -s -o /dev/null -w "%{http_code}" "https://api.github.com/repos/qbcore-framework/${name}")

    if [[ "$responseCode" -eq 404 ]]; then
        printf "Could not find qbcore resource named %s\n" "$name"
        printf "\n"
        return
    fi

    # Navigate to the resource directory
    cd "$resourcePath" || {
        printf "Failed to navigate to %s\n" "$resourcePath"
        return
    }

    # Initialize Git if not already done
    if [[ ! -d ".git" ]]; then
        printf "Initializing Git for %s\n" "$name"
        git clone --no-checkout "https://github.com/qbcore-framework/${name}" tempGit || {
            printf "Git clone failed for %s\n" "$name"
            return
        }
        mv tempGit/.git ./.git
        rm -rf tempGit
    else
        printf "Git already initialized for %s\n" "$name"
    fi

    # Fetch and pull updates
    printf "Fetching and pulling latest updates for %s\n" "$name"
    git fetch || printf "Failed to fetch for %s\n" "$name"
    git pull || printf "Failed to pull for %s\n" "$name"

    printf "\n"
}

# Export the function for use in 'find -exec'
export -f handleResource

# Find and process all matching resource directories
find "$resourceFolder" -type d -name "*qb-*" -exec bash -c 'handleResource "$0"' {} \;

printf "Processing complete.\n"