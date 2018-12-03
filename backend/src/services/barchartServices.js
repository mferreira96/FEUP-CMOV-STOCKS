const api = require('./api')
const config = require('../config/config')

module.exports = {
  retrieveCompaniesQuote (companySymbols) {
    return api.getBarchartApi().get('/getQuote.json', {
      params: {
        symbols: companySymbols,
        apikey: config.stocks.barchartKey,
        mode: 'R'
      }
    })
  }
}
