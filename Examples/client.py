'''
example of using API
'''

from requests import post

#defining features
ABS = 2
ELECTRIC_WINDOWS = 3
SUNROOF = 5
BLUETOOTH = 7
BURGLAR_ALARM = 11
PARKING_CONTROL = 13
NAVIGATION = 17
ON_BOARD_COMPUTER = 19
MULTI_STEERING_WHEEL = 23

#urls
registerUrl = "https://localhost:44325/API/RegisterCar"
updateUrl = "https://localhost:44325/API/UpdateCar"
deleteUrl = "https://localhost:44325/API/DeleteCar"
getUrl = "https://localhost:44325/API/GetCar"

setPropUrl = "https://localhost:44325/API/SetProperties"
getPropUrl = "https://localhost:44325/API/GetProperties"

# registering example cars
BMWResp = post(registerUrl,
            verify=False,
            params = {
                'brand': 'BMW',
                'year': 2010,
                'features': 1, # no features set
                'cost': 55000,
                'curr': 'USD',
                'desc': 'BMW 3 series'
            },
            files = {'img': open('photos/BMW.jpg', 'rb')}
        )

MecedesResp = post(registerUrl,
            verify=False,
            params = {
                'brand': 'Mercedes',
                'year': 2015,
                'features': SUNROOF * BLUETOOTH,
                'cost': 75000,
                'curr': 'EUR',
                'desc': 'Mercedes Cabriolet'
            },
            files = {'img': open('photos/Mercedes.jpg', 'rb')}
        )

# not every field is required
ToyotaResp = post(registerUrl,
            verify=False,
            params = {
                'brand': 'Toyota',
                'year': 2017,
                'features': BLUETOOTH * ON_BOARD_COMPUTER * NAVIGATION,
                'cost': 75000,
                'curr': 'EUR',
            }
        )


# updating info of Mercedes
import json
mid = json.loads(MecedesResp.text)["registeredID"]

post(updateUrl,
        verify=False,
        params = {
                'id': mid,
                'year': 2019 # changing year to 2019
            }
     )

# deleting toyota
tid = json.loads(ToyotaResp.text)["registeredID"]

post(deleteUrl,
        verify=False,
        params = {
                'id': tid
            }
     )

# reading all cars
print("response: ", post(getUrl, verify=False).text)

# changing properties, adding ABS and removing Bluetooth from Mercedes
post(setPropUrl,
        verify=False,
        params = {
                'id': mid,
                'bluetooth': False,
                'abs': True
            }
     )

# reading properties of Mercedes again
print(post(getPropUrl, params = { 'id': mid }, verify=False).text)



