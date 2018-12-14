const api = require('./api')
const config = require('../config/config')

module.exports = {
  retrieveCompanyData (companySymbol, byType) {
    var type = {
      week: 'TIME_SERIES_WEEKLY',
      month: 'TIME_SERIES_MONTHLY',
      day: 'TIME_SERIES_DAILY'
    }

    return api.getAlphaAdvantageApi().get('', {
      params: {
        function: type[byType],
        symbol: companySymbol,
        apikey: config.stocks.alphaVantageKey,
        outputsize: 'compact'
      }
    })
  }
}
