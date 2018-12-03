const alphaVantageServices = require('../services/alphaVantageServices')

module.exports = {
  async retrieveData (req, res) {
    try {
      var companyTick = req.body.comapany_tick
      var response = await alphaVantageServices.retrieveCompany(companyTick)
      res.status(200).send(response.data)
    } catch (error) {
      console.log(error)
      res.status(400).send('Error Sending Data')
    }
  }
}
