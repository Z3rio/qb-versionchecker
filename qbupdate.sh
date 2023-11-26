#!/bin/bash

printf "Input resources folder: "
read resourceFolder

function handleResource () {
    printf "resource: $1\n"
}
export -f handleResource

find $resourceFolder -type d -name "*qb-*" -exec bash -c "handleResource \"{}\"" \;