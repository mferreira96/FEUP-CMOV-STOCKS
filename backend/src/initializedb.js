/* eslint-disable no-unused-vars */
const config = require('./config/config')

function section (section) {
  console.log(`############\n# ${section}\n###########`)
}

module.exports = async (initialize) => {
  section(config.stocks.alphaVantageKey)
  if (!initialize) {
    section('Resume')
    return
  }
  section('Restarting')
}
