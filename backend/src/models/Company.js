module.exports = (sequelize, DataTypes) => {
  const company = sequelize.define('Company', {
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    symbol: {
      type: DataTypes.STRING,
      allowNull: false
    },
    lastPrice: {
      type: DataTypes.DOUBLE
    },
    netChange: {
      type: DataTypes.DOUBLE
    },
    percentChange: {
      type: DataTypes.DOUBLE
    }
  }, {
    freezeTableName: true
  })

  company.associate = (models) => {
    company.hasMany(models.StockData, {
      onDelete: 'CASCADE',
      onUpdate: 'CASCADE'
    })
  }

  return company
}
