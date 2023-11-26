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

    cd "$1"
    if [[ ! -d ".git" ]]; then
        printf "Initializing Git\n"
        git init
        git remote add origin git@github.com:qbcore-framework/${NAME}.git
    else
        printf "Git already initialized\n"
    fi

    git fetch
    git pull

    printf "\n\n"
}
export -f handleResource

find $resourceFolder -type d -name "*qb-*" -exec bash -c "handleResource \"{}\"" \;