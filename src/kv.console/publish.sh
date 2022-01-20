#!/bin/bash

dotnet publish -c Release -r linux-x64 ./kv.console-deploy.csproj
pathTarget="bin/Release/net6.0/linux-x64/publish"
du -h $pathTarget
ls "$pathTarget/kv"

if [[ "$(hostnamectl hostname)" = "bramble" ]]; then
  cp "$pathTarget/kv" ~/scripts/  
fi