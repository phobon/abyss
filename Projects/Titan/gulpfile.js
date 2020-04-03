var gulp = require('gulp');

// Typescript transpiling
var tsc = require('gulp-typescript');
var sourcemaps = require('gulp-sourcemaps');

// Stylesheet transpiling
// TODO: Look at postCSS
var less = require('gulp-less');
var postcss = require('gulp-postcss');
var nano = require('cssnano');
var rename = require('gulp-rename');

gulp.task('tsc', function() {
    var result = gulp.src(['./src/**/*.ts', './src/**/*.tsx', './typings/main/**/*.ts', './typings/pouchdb/*.ts'])    
        .pipe(sourcemaps.init())
        .pipe(tsc({
            noExternalResolve: false,
            target: 'ES5',
            module: "commonjs",
            moduleResolution: "classic",
            jsx: "react"
        }));
        
    return result.js
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('./bin'))
});

gulp.task('less', function() {
    var processors = [nano];    
    gulp.src('./src/stylesheets/titan.less')
        .pipe(less())
        .pipe(postcss(processors))
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest('./bin/stylesheets'))
});

gulp.task('transpile', ['tsc', 'less']);

gulp.task('transpile-watch', function() {
    gulp.watch('./src/stylesheets/**/*.less', ['less']);
    gulp.watch('./src/scripts/**/*.ts', ['tsc']);
});