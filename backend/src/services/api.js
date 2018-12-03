const axios = require('axios')
const config = require('../config/config')

module.exports = {
  getAlphaAdvantageApi () {
    return axios.create({
      baseURL: config.stocks.alphaVantageURL
    })
  },
  getBarchartApi () {
    return axios.create({
      baseURL: config.stocks.barchartURL
    })
  }
}
