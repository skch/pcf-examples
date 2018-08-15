#!/bin/sh
cd ../rest-api
cfg=appsettings.json
tmp=../tmp.json
gver=`git describe --tags`
cp $cfg $tmp
sed -e "s;%VERSION%;$gver;g" $tmp > $cfg
dotnet build
cf push
cp $tmp $cfg
