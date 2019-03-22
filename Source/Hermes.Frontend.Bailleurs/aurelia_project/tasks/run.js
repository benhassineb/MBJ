import gulp from 'gulp';
import path from 'path';
import browserSync from 'browser-sync';
import historyApiFallback from 'connect-history-api-fallback/lib';
import project from '../aurelia.json';
import build from './build';
import watch from './watch';

const sslPath = path.join(project.platform.baseDir, '../aurelia_project/tasks/ssl');

// const bs = browserSync.create();

let serve = gulp.series(
  build,
  done => {
    browserSync({
      online: true,
      open: 'external',
      browser: ['chrome'],
      host: project.run.host,
      port: 80,
      // port: 443,
      // https: {
      //   key: path.join(sslPath, 'lvh.me.key'),
      //   cert: path.join(sslPath, 'lvh.me.crt')
      // },
      logLevel: 'silent',
      server: {
        baseDir: [project.platform.baseDir],
        middleware: [historyApiFallback(), (req, res, next) => {
          res.setHeader('Access-Control-Allow-Origin', '*');
          next();
        }]
      }
    }, (err, cb) => {
      if (err) return done(err);
      let urls = cb.options.get('urls').toJS();
      log(`Application Available At: ${urls.external}`);
      log(`BrowserSync Available At: ${urls.ui}`);
      done();
    });
  }
);

function log(message) {
  console.log(message); //eslint-disable-line no-console
}

function reload() {
  log('Refreshing the browser');
  browserSync.reload();
}

let run = gulp.series(
  serve,
  done => {
    watch(reload);
    done();
  }
);

export default run;
