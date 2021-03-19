## SimpleCarAPI
Example Of Simple CRUD API

---
_Note: Please Change Database Connection String Located In `SimpleCarAPI > Source > CarAPI > DatabaseConnector.cs` Before Executing Program_

---
#### Important Routes:
* [/API/*](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/Controllers/MainHandler.cs) - CRUD Pages
    * [/API/RegisterCar](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/CrudOperations/Registration.cs) - Serves Car Registration
    * [/API/UpdateCar](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/CrudOperations/Updation.cs) - Server Modification Of Car Record
    * [/API/DeleteCar](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/CrudOperations/Deletion.cs) - Serves Deletion Of Car Record
    * [/API/GetCar](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/CrudOperations/Selection.cs) - Can Be Used To View Info Of The Selected Car Or All Cars Registered
    * [/API/GetProperties](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/CrudOperations/PropertySelection.cs) - To List All Features Of Selected Car
    * [/API/SetProperties](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/CrudOperations/PropertyUpdation.cs) - To Modify/Delete/Add Features Of Selected Car
* [/IMG/*](https://github.com/Cool-API-Inc/SimpleCarAPI/blob/main/Source/CarAPI/Controllers/ImageHandler.cs) - Uploaded Images 

---

### Registering Car
Adds car record and returns registration ID, (along with return code and success status)

##### POST Parameters:
1. brand: brand parameter (one of the valid) `[required]`
2. year: build year parameter `[required]`
3. desc: description parameter (unicode accepted) 
4. img: image file to upload
5. features: features value `[required]`
6. cost: car cost `[required]`
7. curr: cost currency (one of the valid) `[required]`

(4) - image file which will be uploaded on the server and be accessible using the url returned  
(5) - features value (single integer), multiplication of feature codes

##### Python Example:
``` python
requests.post('https://{host}:{port}/API/RegisterCar', verify=False,
    params = { 
        'brand': 'BMW',
        'year': 2010,
        'features': ABS * PARKING_CONTROL,
        'cost': 55000,
        'curr': 'USD',
        'desc': 'BMW 3 series'
    },
    files = {'img': open('photos/BMW.jpg', 'rb')}
) 
```

##### Feature Codes:
* ABS = 2
* ELECTRIC_WINDOWS = 3
* SUNROOF = 5
* BLUETOOTH = 7
* BURGLAR_ALARM = 11
* PARKING_CONTROL = 13
* NAVIGATION = 17
* ON_BOARD_COMPUTER = 19
* MULTI_STEERING_WHEEL = 23

---

### Updating Car
Requires id parameter, every other parameter is optional, if set, value will be updated. 
Returns number of fields affected (along with return code and success status).

##### Python Example:
``` python
requests.post('https://{host}:{port}/API/UpdateCar', verify=False,
    params = {
        'id': 'xxx-xxx-xxx',
        'year': 2019  # changing year to 2019
    }
)
```
---

### Deleting Car 
Requires single parameter (id) to remove car record and returns success status.

---

### Selecting Car(s)
If __id__  not specified, returns list of registered cars (along with success status). Else, list contains only the element with speficied __id__.

---

### Updating Features List
Requires id parameter and takes optional booleans, if set, adds or removes corresponding feature (adds if "true" and removes if "false"). Returns number of features affected (along with success status).

##### POST Parameters:
1. id: the car to modify `[required]`
3. abs: corresponds to ABS feature 
4. ewindow: corresponds to ELECTRIC_WINDOWS feature
5. sunroof: corresponds to SUNROOF feature
6. bluetooth: corresponds to BLUETOOTH feature
7. alarm: corresponds to BURGLAR_ALARM feature
8. parkingCtrl: corresponds to PARKING_CONTROL feature
9. navigation: corresponds to NAVIGATION feature
10. boardComputer: corresponds to ON_BOARD_COMPUTER feature
11. mltwheel: corresponds to MULTI_STEERING_WHEEL feature

##### Python Example:
``` python
requests.post('https://{host}:{port}/API/SetProperties', verify=False,
    params = {
        'id': 'xxx-xxx-xxx',
        'bluetooth': False, # removing Bluetooth
        'abs': True # adding ABS
    }
)
```

---

### Reading Features List
Requires id parameter and returns list of features for selected car in both formats (along with success status).

