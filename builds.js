

const fs = require('fs');
const path = require('path');
const { exec } = require('child_process');

// Recursive directory copy
const copyRecursiveSync = (src, dest) => {
  if (fs.existsSync(src)) {
    fs.mkdirSync(dest, { recursive: true });
    fs.readdirSync(src).forEach(file => {
      const curSrc = path.join(src, file);
      const curDest = path.join(dest, file);
      fs.statSync(curSrc).isDirectory() 
        ? copyRecursiveSync(curSrc, curDest)
        : fs.copyFileSync(curSrc, curDest);
    });
  }
};

const buildCallback = (error) => {
  if (error) {
    console.error(`🚨 Build failed: ${error}`);
    process.exit(1);
  }

  console.log('✅ Expo web build complete. Copying Unity files...');
  
  // Path configuration
  const expoBuildDir = path.resolve('web-build');
  const finalBuildDir = path.resolve('../docs');
  const unitySource = path.resolve('public/Build');
  const unityTarget = path.join(finalBuildDir, 'Build');

  // Copy Expo build
  copyRecursiveSync(expoBuildDir, finalBuildDir);
  
  // Copy Unity files
  copyRecursiveSync(unitySource, unityTarget);

  // Add GitHub Pages requirement
  fs.writeFileSync(path.join(finalBuildDir, '.nojekyll'), '');

  console.log('🚀 All files ready in ../docs/');
};

// Start build process
console.log('🏗 Starting Expo web build...');
exec('expo build:web', buildCallback);
