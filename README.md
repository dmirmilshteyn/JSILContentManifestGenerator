# JSIL Content Manifest Generator

For use with the [JSIL compiler](https://github.com/sq/jsil).

## Usage

Two command line inputs are required:
* Base directory
* Input child paths

This application will generate a content manifest for all files in the specified input directories and all child directories.

 The generated file will be placed in ```Content.manifest.js```.
 
### Example
 
 ```./JSILContentManifestGenerator.exe --base "C:\Content" --paths "Assets" "Configuration"```
 
 A ```Content.manifest.js``` file will be placed in ```C:\Content```. This manifest file will include the required metadata for all files in the ```C:\Content\Assets``` and ```C:\Content\Configuration``` folders.
