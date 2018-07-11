#!/bin/bash

set -ex

newman run $COLLECTION_URL --folder api -e $ENVIRONMENT_URL --bail --no-color --reporters cli
