#!/bin/bash

printf "Input resources folder: "
read resourceFolder

if [[ ! -d "$resourceFolder" ]]; then
    printf "\nInvalid folder path\n"
    exit;
fi

function handleResource () {
    NAME="$(basename $1)"
    printf "Handling: ${NAME}\n"

    responseCode=$(curl -s -o /dev/null -w "%{http_code}" "https://api.github.com/repos/qbcore-framework/${NAME}")

    if [[ ! "$response" -eq 404 ]]; then
        cd "$1"
        if [[ ! -d ".git" ]]; then
            printf "Initializing Git\n"
            git clone --no-checkout https://github.com/qbcore-framework/${NAME} tempGit
            mv tempGit/.git ./.git
            rm -rf tempGit
        else
            printf "Git already initialized\n"
        fi

        git fetch
        git pull
    else
        echo "Couldnt find qbcore resource named ${NAME}"
    fi

    printf "\n\n"
}
export -f handleResource

find "$resourceFolder" -type d -name "*qb-*" -exec bash -c 'handleResource "$0"' {} \;