#!/bin/bash

DIR="$(dirname $0)"

python3 "${DIR}/CodeGenerator.py" $1 $2

if [ $? -eq 0 ]; then
    echo "== Code Generation succeeded =="
    read -n1 -r -p "Press any key to continue..." key
    exit 0
else
    echo "== Code Generation failed =="
    read -n1 -r -p "Press any key to continue..." key
    exit 1
fi
