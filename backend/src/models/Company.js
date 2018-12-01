module.exports = (sequelize, DataTypes) => {
  const company = sequelize.define('Company', {
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    tick: {
      type: DataTypes.STRING,
      allowNull: false
    },
    price: {
      type: DataTypes.DOUBLE,
      allowNull: false
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
