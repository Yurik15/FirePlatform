module.exports = merge(prodEnv, {
    NODE_ENV: '"production"',
    API_ENDPOINT: '"https://localhost:44358/api"'
  })