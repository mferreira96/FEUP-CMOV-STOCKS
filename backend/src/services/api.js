const axios = require('axios')
const config = require('../config/config')

module.exports = () => {
  return axios.create({
    baseURL: config.stocks.alphaVantageURL
  })
}
