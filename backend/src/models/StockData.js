module.exports = (sequelize, DataTypes) => {
  const stockData = sequelize.define('StockData', {
    value: {
      type: DataTypes.DOUBLE,
      allowNull: false
    },
    date: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    freezeTableName: true
  })

  stockData.associate = (models) => {
    stockData.belongsTo(models.Company, {
      onDelete: 'CASCADE',
      onUpdate: 'CASCADE'
    })
  }
  return stockData
}
