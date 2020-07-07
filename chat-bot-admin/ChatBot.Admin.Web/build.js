module.exports = function task(gulp, plugins, vendors, callback) {

    var ENVIRONMENT_LOCAL = 'local';
    var ENVIRONMENT_DEV = 'dev';
    var ENVIRONMENT_PROD = 'prod';
    var ENVIRONMENT_PRE = 'pre';
    var ENVIRONMENT_PSI = 'psi';
    var ENVIRONMENT_TEST = 'test';

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

    function types() {
        return gulp.src([__dirname + '/src/typings/index.d.ts', __dirname + '/src/typings/index.min.d.ts'])
            .pipe(gulp.dest(__dirname + '/dist'))
    };

    function styles() {
        return gulp.src(__dirname + '/src/sass/*.scss')
            .pipe(plugins.sass().on('error', plugins.sass.logError))
            .pipe(plugins.concat('main.css'))
            .pipe(gulp.dest(__dirname + '/dist'))
    };

    function replaceEnvironmentVars() {
        var jsonEnvSettings = JSON.parse(plugins.fs.readFileSync(__dirname + '/src/environment/config-' + getEnvironmentName() + '.json', 'utf8'));
        return plugins.replace({
            patterns: [
                {
                    json: jsonEnvSettings
                }
            ]
        });
    }

    function build(resolve) {

        types();
        styles();

        var standalone = plugins.browserify(__dirname + '/src/ts/main.tsx', {
            standalone: 'Main'
        })
            .add(__dirname + '/src/ts/main.tsx')
            .plugin(plugins.tsify, { noImplicitAny: false })
            .transform(plugins.babelify.configure({
                plugins: [plugins.bpoa]
            }))

        vendors.forEach(function (pkg) {
            standalone.exclude(pkg);
            standalone.external(pkg);
        });

        return standalone.bundle()
            .on('error', function (e) {
                plugins.gutil.log('Browserify Error: but ignore it!', e);
            })
            .pipe(plugins.source('index.js'))
            .pipe(plugins.buffer())
            .pipe(replaceEnvironmentVars())
            .pipe(gulp.dest(__dirname + '/dist'))
            .pipe(plugins.rename('index.min.js'))
            .pipe(plugins.buffer())
            .pipe(replaceEnvironmentVars())
            .pipe(plugins.uglify())
            .pipe(gulp.dest(__dirname + '/dist')).on('end', function () {
                //callback();
                resolve();
            });
    }

    return new Promise(build);
};
