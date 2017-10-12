#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
index=$1
noclsArg=$2
runtime=$3
deployDir=../.deploy/codegen
distDir=../dist
ubuntu16=ubuntu.16.04-x64
zipFile="codegen.$runtime.zip"

if [[ "$index" == "" ]]; 	then index="1";fi
if [[ "$noclsArg" == "" ]]; then clear;fi
if [[ "$runtime" == "" ]]; 	then buildFailed;fi
if [ ! -d "$distDir" ]; 	then mkdir $distDir;fi

block "$index - PUBLISH AND ZIP CODEGEN FOR $C_CYAN $runtime $C_TRANSPARENT"

execute "dotnet publish ../src/Paradigm.CodeGen.UI.Console/Paradigm.CodeGen.UI.Console.csproj -c Release -r $runtime -o ../$deployDir/$runtime/"

execute "rm -rf $distDir/$zipFile"
pushd "$deployDir/$runtime/"
execute "zip -9 ../../$distDir/$zipFile ./*"
popd

buildSuccessfully