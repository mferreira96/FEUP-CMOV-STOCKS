require('dotenv').config()
const express = require('express')
const bodyParser = require('body-parser')
const cors = require('cors')
const morgan = require('morgan')
const {sequelize} = require('./models')
const config = require('./config/config')
const initializedb = require('./initializedb')

const app = express()
app.use(morgan('combined'))
app.use(bodyParser.json())
app.use(cors())

const reload = true

require('./routes')(app)

/*
* Clear Database
  sequelize.sync({force: true})
  Careful!!
*/

sequelize.sync({force: reload})
  .then(async () => {
    await initializedb(reload)
  })
  .then(() => {
    app.listen(process.env.PORT || config.port)
    console.log(`Server started on port ${config.port}`)
  })

module.exports = app
