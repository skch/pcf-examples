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
cp $vault $cfgvault
sed -e "s;%PRIVATEKEY%;$gver;g" $cfgvault > $vault

# Build and push
dotnet build
cf push

# Restore files
cp $cfgtmp $cfg
cp $vaulttmp $vault
