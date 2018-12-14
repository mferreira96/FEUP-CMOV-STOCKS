const alphaVantageServices = require('../services/alphaVantageServices')

module.exports = {
  async retrieveData (req, res) {
    try {
      var companyTick = req.params.companies
      var companies = companyTick.split(',')
      var weekly = req.params.type

      var companyList = []

      var size = 30

      for (var company of companies) {
        var response = await alphaVantageServices.retrieveCompanyData(company, weekly)
        var keys = Object.keys(response.data)
        var series = response.data[keys[1]]
        var companyElement = {
          symbol: company,
          History: []
        }

        var count = size
        var cached = null
        for (var date in series) {
          if (--count < 0) break
          if (cached === null) {
            var seriesKeys = Object.keys(series[date])
            cached = seriesKeys[3]
          }

          var value = series[date][cached]

          companyElement.History.push({
            Value: value,
            Date: date
          })
        }
        companyList.push(companyElement)
      }
      res.status(200).send({companies: companyList})
    } catch (error) {
      console.log(error)
      res.status(400).send('Error Sending Data')
    }
  }
}
