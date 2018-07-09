#!/bin/bash

set -ex

pushd repo
    pushd $PROJECT_NAME
        dotnet test
    popd
popd
