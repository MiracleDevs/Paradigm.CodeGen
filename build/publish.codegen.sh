#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
index=$1
noclsArg=$2
win86="win-x86"
win64="win-x64"
linux="linux-x64"
osx="osx-x64"

if [[ "$index" == "" ]]; then index="1"; fi
if [[ "$noclsArg" == "" ]]; then clear; fi

block "$index - PUBLISH CODEGEN"

#------------------------------------------------
# BUILD SOLUTION
#------------------------------------------------
execute "bash ./build.solution.sh $index.1 nocls"

#------------------------------------------------
# PUBLISH CODEGEN
#------------------------------------------------
execute "bash ./publish.runtime.sh $index.2 nocls $win86"
execute "bash ./publish.runtime.sh $index.3 nocls $win64"
execute "bash ./publish.runtime.sh $index.4 nocls $linux"
execute "bash ./publish.runtime.sh $index.5 nocls $osx"

buildSuccessfully