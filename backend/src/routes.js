const stocksController = require('./controllers/stocksController')
module.exports = (app) => {
  // *****************
  // * Costumers
  // *****************
  app.get('/stock',
    stocksController.retrieveData)
}
