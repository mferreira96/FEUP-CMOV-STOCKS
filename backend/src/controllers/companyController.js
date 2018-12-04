const {Company} = require('../models')
// const alphaVantageServices = require('../services/alphaVantageServices')
const barchartServices = require('../services/barchartServices')

module.exports = {
  async updateCompanies (req, res) {
    try {
      for (var i = 0; i < req.body.companies.length; i++) {
        await Company.update({
          price: req.companies[i].price
        },
        {
          where: { tick: req.body.companies[i].tick }
        })
      }

      res.status(200).send({msg: 'succes'})
    } catch (error) {
      console.log(error)
      res.status(400).send('Error Sending Data')
    }
  },
  async getCompanies (req, res) {
    try {
      var result = await Company.findAll()

      var companyTicks = []
      var dbCompanies = []

      for (var i = 0; i < result.length; i++) {
        dbCompanies.push(result[i].dataValues)
        companyTicks.push(result[i].dataValues.symbol)
      }

      var companyTicksString = companyTicks.join(',')

      await barchartServices.retrieveCompaniesQuote(companyTicksString).then(function (resp) {
        var companies = []

        if (resp.data.status.code !== 200) {
          companies = dbCompanies
        } else {
          companies = resp.data.results

          for (var i = 0; i < companies.length; i++) {
            Company.update({
              lastPrice: companies[i].lastPrice,
              netChange: companies[i].netChange,
              percentChange: companies[i].percentChange
            },
            {
              where: { symbol: companies[i].symbol }
            })
          }
        }

        res.status(200).send({companies})
      }).catch(function (resp) {
        res.status(400).send({msg: 'error', data: resp})
        throw new Error(resp.statusText || resp.status || 'Bad request')
      })
    } catch (error) {
      res.status(400).send({msg: 'error', data: error})
    }
  }
}
