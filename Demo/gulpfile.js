var gulp = require('gulp-param')(require('gulp'), process.argv),
    browserSync = require('browser-sync').create(),
    replace = require('gulp-replace'),
    rename = require('gulp-rename'),

    jsConcat = require('gulp-concat'),

    cleanCSS = require('gulp-clean-css'),
    concatCSS = require('gulp-concat-css'),
    gulpLess = require('gulp-less');


gulp.task('browser-sync', function () {
    browserSync.init();
});

gulp.task('scripts-core', function () {
    gulp.src([
        'wwwroot/Dev/Scripts/bower_components/jQuery/dist/jquery.js',
        'wwwroot/Dev/Scripts/bower_components/handlebars/handlebars.js'
    ])
        .pipe(jsConcat('app.libs.min.js'))
        .pipe(gulp.dest('wwwroot/Build/Scripts/'));
});

gulp.task('scripts', function () {
    return gulp.src([
        '!wwwroot/Dev/Scripts/bower_components/**/*',
        'wwwroot/Dev/Scripts/Data/*.js',
        'wwwroot/Dev/Scripts/Extensions/*.js',
        'wwwroot/Dev/Scripts/Entities/*.js',
        'wwwroot/Dev/Scripts/Services/*.js',
        'wwwroot/Dev/Scripts/Helpers/**/*.js',
        'wwwroot/Dev/Scripts/Components/*.js',
        'wwwroot/Dev/Scripts/Sections/*.js',
        'wwwroot/Dev/Scripts/Banners/*.js',
        'wwwroot/Dev/Scripts/Sliders/*.js',
        'wwwroot/Dev/Scripts/main.js'
    ])
        .pipe(jsConcat('app.min.js'))
        .pipe(gulp.dest('wwwroot/Build/Scripts/'));
});

gulp.task('less', function () {
    return gulp.src(['!wwwroot/Dev/Styles/login.less', 'wwwroot/Dev/Styles/**/*.less'])
        .pipe(gulpLess())
        .pipe(gulp.dest('wwwroot/Dev/Styles/'));
});

gulp.task('styles-core', ['less'], function () {
    return gulp.src('wwwroot/Dev/Styles/Libs/**/*.css')
        .pipe(concatCSS('app.libs.css'))
        .pipe(replace('/*!', '/*'))
        .pipe(cleanCSS())
        .pipe(rename('app.libs.min.css'))
        .pipe(gulp.dest('wwwroot/Build/Styles/'))
        .pipe(browserSync.stream());
});

gulp.task('styles', ['less'], function () {
    return gulp.src(['!wwwroot/Dev/Styles/Libs/**/*.css', 'wwwroot/Dev/Styles/main.css'])
        .pipe(concatCSS('app.css'))
        .pipe(cleanCSS())
        .pipe(rename('app.min.css'))
        .pipe(gulp.dest('wwwroot/Build/Styles/'))
        .pipe(browserSync.stream());
});

gulp.task('watch', ['styles', 'scripts'], function () {
    gulp.watch(['wwwroot/Dev/Styles/**/*.less', 'wwwroot/Dev/Styles/**/**/*.less', '!wwwroot/Dev/Styles/libs/*.less'], ['styles']);
    gulp.watch('wwwroot/Dev/Styles/libs/*.less', ['styles-core']);
    gulp.watch(['wwwroot/Dev/Scripts/**/*.ts', 'wwwroot/Dev/Scripts/JS/*.js'], ['scripts']);
});

gulp.task('default', ['styles', 'scripts', 'watch']);