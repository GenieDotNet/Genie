#!/usr/bin/env bash
dotnet publish /home/travis/rusith/Genie/GenieCLI -c release -r win10-x64
dotnet publish /home/travis/rusith/Genie/GenieCLI -c release -r ubuntu.16.10-x64
dotnet publish /home/travis/rusith/Genie/GenieCLI -c release -r osx.10.11-x64
zip -r genieCLI_win-x64-${TRAVIS_TAG}.zip ${TRAVIS_BUILD_DIR}/GenieCLI/bin/release/netcoreapp1.1/win10-x64/publish
zip -r genieCLI_linux-x64-${TRAVIS_TAG}.zip ${TRAVIS_BUILD_DIR}/GenieCLI/bin/release/netcoreapp1.1/ubuntu.16.10-x64/publish
zip -r genieCLI_osx-x64-${TRAVIS_TAG}.zip ${TRAVIS_BUILD_DIR}/GenieCLI/bin/release/netcoreapp1.1/osx.10.11-x64/publish