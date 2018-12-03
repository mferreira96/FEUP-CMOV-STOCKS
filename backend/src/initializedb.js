/* eslint-disable no-unused-vars */
const config = require('./config/config')
const {Company, StockData} = require('./models')

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

  await Company.create({
    name: 'Facebook',
    symbol: 'FB'
  })

  await Company.create({
    name: 'Apple',
    symbol: 'AAPL'
  })

  await Company.create({
    name: 'IBM',
    symbol: 'IBM'
  })

  await Company.create({
    name: 'Microsoft',
    symbol: 'MSFT'
  })

  await Company.create({
    name: 'Oracle',
    symbol: 'ORCL'
  })

  await Company.create({
    name: 'Google',
    symbol: 'GOOG'
  })

  await Company.create({
    name: 'Twitter',
    symbol: 'TWTR'
  })

  await Company.create({
    name: 'Intel',
    symbol: 'INTC'
  })

  await Company.create({
    name: 'AMD',
    symbol: 'AMD'
  })

  await Company.create({
    name: 'Hewlett Packard',
    symbol: 'HPE'
  })

  await Company.create({
    name: 'Amazon',
    symbol: 'AMZN'
  })

  await Company.create({
    name: 'Tesla',
    symbol: 'TSLA'
  })

  await Company.create({
    name: 'Netflix',
    symbol: 'NFLX'
  })
}
