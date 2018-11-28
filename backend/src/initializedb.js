/* eslint-disable no-unused-vars */
function section (section) {
  console.log(`############\n# ${section}\n###########`)
}

module.exports = async (initialize) => {
  if (!initialize) {
    section('Resume')
    return
  }
  section('Restarting')
}
