#!/bin/sh
cd ../rest-api

# Update Configuration File
cfg=appsettings.json
cfgtmp=../tmp.json
gver=`git describe --tags`
cp $cfg $cfgtmp
sed -e "s;%VERSION%;$gver;g" $cfgtmp > $cfg

# Update Vault File
vault=Models/AppVault.cs
vaulttmp=../vault.cs
pkey=`cat ../privatekey.md`
cp $vault $vaulttmp
sed -e "s;%PRIVATEKEY%;$pkey;g" $vaulttmp > $vault

# Build and push
dotnet build
cf push

# Restore files
cp $cfgtmp $cfg
cp $vaulttmp $vault
