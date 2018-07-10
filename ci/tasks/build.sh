#!/bin/bash

set -ex

pushd repo
    dotnet publish $PROJECT_NAME -o ../../published
popd