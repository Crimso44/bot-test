var gulp = require('gulp');
var uglifyify = require('uglifyify');
var tsify = require('tsify');
var htmlreplace = require('gulp-html-replace');
var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var browserify = require('browserify');
var babelify = require('babelify');
var concat = require('gulp-concat');
var sass = require('gulp-sass');
var sourcemaps = require('gulp-sourcemaps');
var uglify = require('gulp-uglify');
var reactify = require('reactify');
var clean = require('gulp-clean');
var hash = require('gulp-hash');
var filenames = require("gulp-filenames");
var replace = require('gulp-replace-task');
var fs = require('fs');

var ENVIRONMENT_LOCAL = 'local';
var ENVIRONMENT_DEV = 'dev';
var ENVIRONMENT_PROD = 'prod';
var ENVIRONMENT_PRE = 'pre';
var ENVIRONMENT_PSI = 'psi';
var ENVIRONMENT_TEST = 'test';

var path = {
    MINIFIED_OUT: 'app.min.js',
    OUT: 'app.js',
    DEST_SRC: 'dist/js',
    ENTRY_POINT: 'src/ts/main.tsx',
    SASS_SRC: './src/sass/*.scss',
    CSS_DIST: 'dist/styles',
    IMAGES_SRC: 'src/images/*.*',
    IMAGES_DIST: 'dist/images',
    FONTS_SRC: 'src/fonts/*.*',
    FONTS_DIST: 'dist/fonts',
    INDEX_SRC: 'src/index.html',
    INDEX_DIST: 'dist',
    LIB_DIST: 'lib.js',
    VENDORS: [
        'react',
        'react-dom',
        'es6-promise',
        // 'react-addons-shallow-compare',
        'react-router',
        'redux',
        'react-redux',
        'redux-actions',
        'react-router-redux',
        'history',
        //'jquery',
        //'jdenticon',
        'lodash',
        'axios',
        'moment',
        'moment/locale/ru',
        //'react-dates2',
        //'js-polyfills',
        'react-table'
    ]
};

function getEnvironmentName() {
    var aspnetcoreEnvironment = process.env.ASPNETCORE_ENVIRONMENT;
    switch (aspnetcoreEnvironment) {
        case ENVIRONMENT_LOCAL:
            return aspnetcoreEnvironment;
        case ENVIRONMENT_DEV:
            return aspnetcoreEnvironment;
        case ENVIRONMENT_PROD:
            return aspnetcoreEnvironment;
        case ENVIRONMENT_TEST:
            return aspnetcoreEnvironment;
        case ENVIRONMENT_PSI:
            return aspnetcoreEnvironment;
        case ENVIRONMENT_PRE:
            return aspnetcoreEnvironment;
        default:
            return 'dev';
    }
}

function replaceEnvironmentVars() {
    var jsonEnvSettings = JSON.parse(fs.readFileSync('src/environment/config-' + getEnvironmentName() + '.json', 'utf8'));
    return replace({
        patterns: [
            {
                json: jsonEnvSettings
            }
        ]
    });
}


gulp.task('vendors', function () {
    return gulp.src([
        './node_modules/promise-polyfill/promise.min.js',
        './node_modules/js-polyfills/polyfill.min.js',
        './src/vendors/keycloak.js'
    ], {allowEmpty: true})
        .pipe(gulp.dest(path.DEST_SRC));
});


gulp.task('clean', function () {
    return gulp.src(path.INDEX_DIST, { read: false, allowEmpty: true })
        .pipe(clean());
});

gulp.task('copy-index-html', function () {
    return gulp.src(path.INDEX_SRC)
        .pipe(gulp.dest(path.INDEX_DIST));
});

gulp.task('copy-fonts', function () {
    return gulp.src(path.FONTS_SRC)
        .pipe(gulp.dest(path.FONTS_DIST));
});

gulp.task('copy-images', function () {
    return gulp.src(path.IMAGES_SRC)
        .pipe(gulp.dest(path.IMAGES_DIST));
});

gulp.task('copy-css', function () {
    return gulp.src(
        [
            './node_modules/perfect-scrollbar/css/perfect-scrollbar.css',
            './node_modules/@sbt/react-ui-components/dist/css/index.css',
            './node_modules/react-table/react-table.css'
        ], {allowEmpty: true})
        .pipe(gulp.dest(path.CSS_DIST))
});

gulp.task('sass', function () {
    return gulp.src(path.SASS_SRC)
        .pipe(sass().on('error', sass.logError))
        .pipe(concat('main-portal.css'))
        .pipe(concat('main.css'))
        .pipe(hash())
        .pipe(gulp.dest(path.CSS_DIST))
        .pipe(filenames("css"));
});

gulp.task('sass-dev', function () {
    return gulp.src(path.SASS_SRC)
        .pipe(sass().on('error', sass.logError))
        .pipe(concat('main-portal.css'))
        .pipe(concat('main.css'))
        .pipe(gulp.dest(path.CSS_DIST))
});

gulp.task('lib', () => {
    const b = browserify({
        debug: true
    });

    path.VENDORS.forEach(lib => {
        b.require(lib);
    });

    return b.bundle()
        .pipe(source(path.LIB_DIST))
        .pipe(buffer())
        .pipe(replaceEnvironmentVars())
        .pipe(uglify())
        .pipe(hash())
        .pipe(gulp.dest(path.DEST_SRC))
        .pipe(filenames("lib"));
});

gulp.task('lib-dev', () => {
    const b = browserify({
        debug: true
    });

    path.VENDORS.forEach(lib => {
        b.require(lib);
    });

    return b.bundle()
        .pipe(source(path.LIB_DIST))
        .pipe(buffer())
        .pipe(replaceEnvironmentVars())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(sourcemaps.write('./maps'))
        .pipe(gulp.dest(path.DEST_SRC));
});

gulp.task('html', function () {
    console.log("files: ", JSON.stringify(filenames.get("lib")), JSON.stringify(filenames.get("js")), JSON.stringify(filenames.get("css")));
    return gulp.src(path.INDEX_SRC)
        .pipe(htmlreplace({
            'js': {
                src: filenames.get("js")[0],
                tpl: '<script src="/js/%s"></script>'
            },
            'lib': {
                src: filenames.get("lib")[0],
                tpl: '<script src="/js/%s"></script>'
            },
            'css': {
                src: filenames.get("css")[0],
                tpl: '<link rel="stylesheet" type="text/css" href="/styles/%s">'
            },
        }))
        .pipe(gulp.dest(path.INDEX_DIST));
});

gulp.task('html-dev', function () {
    return gulp.src(path.INDEX_SRC)
        .pipe(htmlreplace({
            'js': {
                src: "app.js",
                tpl: '<script src="/js/%s"></script>'
            },
            'lib': {
                src: "lib.js",
                tpl: '<script src="/js/%s"></script>'
            },
            'css': {
                src: "main.css",
                tpl: '<link rel="stylesheet" type="text/css" href="/styles/%s">'
            },
        }))
        .pipe(gulp.dest(path.INDEX_DIST));
});

gulp.task('dev', function () {
    return browserify({
        debug: true,
        entries: [path.ENTRY_POINT],
        cache: {},
        packageCache: {}
    })
        .add(path.ENTRY_POINT)
        .plugin(tsify, { noImplicitAny: false, sourceMap: true })
        .external(path.VENDORS)
        .transform(babelify)
        .bundle()
        .pipe(source(path.OUT))
        .pipe(buffer())
        .pipe(replaceEnvironmentVars())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(sourcemaps.write('./maps'))
        .pipe(gulp.dest(path.DEST_SRC));
});

gulp.task('prod', function () {
    return browserify()
        .add(path.ENTRY_POINT)
        .plugin(tsify, { noImplicitAny: false })
        .external(path.VENDORS)
        .transform(reactify)
        .bundle()
        .pipe(source(path.MINIFIED_OUT))
        .pipe(buffer())
        .pipe(replaceEnvironmentVars())
        .pipe(uglify())
        .pipe(hash())
        .pipe(gulp.dest(path.DEST_SRC))
        .pipe(filenames("js"));
});

gulp.task('dev-build', gulp.series('dev', 'sass-dev', 'lib-dev', 'html-dev', 'copy-fonts', 'copy-images', 'copy-css', 'vendors'));
gulp.task('prod-build', gulp.series('prod', 'lib', 'sass', 'html', 'copy-fonts', 'copy-images', 'copy-css', 'vendors'));

gulp.task('build:dev', gulp.series('clean', 'dev-build'));
gulp.task('build:prod', gulp.series('clean', 'prod-build'));
