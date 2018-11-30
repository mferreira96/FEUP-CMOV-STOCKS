const alphaVantageServices = require('../services/alphaVantageServices')

module.exports = {
  async retrieveData (req, res) {
    try {
      var response = await alphaVantageServices.retrieveCompany('FB')
      res.status(200).send(response.data)
    } catch (error) {
      console.log(error)
      res.status(400).send('Error Sending Data')
    }
  }
}
