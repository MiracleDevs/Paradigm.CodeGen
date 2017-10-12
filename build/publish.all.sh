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

block "$index - PUBLISH ALL TOOLS"

#------------------------------------------------
# BUILD SOLUTION
#------------------------------------------------
execute "bash ./build.solution.sh $index.1 nocls"

#------------------------------------------------
# PUBLISH CODEGEN
#------------------------------------------------
execute "bash ./publish.tool.sh codegen ../src/Paradigm.CodeGen.UI.Console/Paradigm.CodeGen.UI.Console.csproj $index.1 nocls"

