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
    tick: 'FB'
  })

  await Company.create({
    name: 'Apple',
    tick: 'AAPL'
  })

  await Company.create({
    name: 'IBM',
    tick: 'IBM'
  })

  await Company.create({
    name: 'Microsoft',
    tick: 'MSFT'
  })

  await Company.create({
    name: 'Oracle',
    tick: 'ORCL'
  })

  await Company.create({
    name: 'Google',
    tick: 'GOOG'
  })

  await Company.create({
    name: 'Twitter',
    tick: 'TWTR'
  })

  await Company.create({
    name: 'Intel',
    tick: 'INTC'
  })

  await Company.create({
    name: 'AMD',
    tick: 'AMD'
  })

  await Company.create({
    name: 'Hewlett Packard',
    tick: 'HPE'
  })

  await Company.create({
    name: 'Amazon',
    tick: 'AMZN'
  })

  await Company.create({
    name: 'Tesla',
    tick: 'TSLA'
  })

  await Company.create({
    name: 'Netflix',
    tick: 'NFLX'
  })
}
