const stocksController = require('./controllers/stocksController')
const companyController = require('./controllers/companyController')

module.exports = (app) => {
  // *****************
  // * Costumers
  // *****************
  app.get('/stocks/:type/:companies',
    stocksController.retrieveData)

  app.get('/companies',
    companyController.getCompanies)
}
