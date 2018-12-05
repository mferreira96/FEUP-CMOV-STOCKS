const api = require('./api')
const config = require('../config/config')

module.exports = {
  retrieveCompanyData (companySymbol, byWeek) {
    return api.getAlphaAdvantageApi().get('', {
      params: {
        function: byWeek ? 'TIME_SERIES_WEEKLY' : 'TIME_SERIES_MONTHLY',
        symbol: companySymbol,
        apikey: config.stocks.alphaVantageKey,
        outputsize: 'compact'
      }
    })
  }
}
