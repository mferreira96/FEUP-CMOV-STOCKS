const api = require('./api')
const config = require('../config/config')

module.exports = {
  retrieveCompany (companySymbol) {
    return api().get('', {
      params: {
        function: 'TIME_SERIES_WEEKLY',
        symbol: companySymbol,
        apikey: config.stocks.alphaVantageKey
      }
    })
  }
}