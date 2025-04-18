# SkillAssessmentPlatform.API

\*\*Version: 1.0

==Applicant account==

```
{
  "email": "az.ab.ei.3813@gmail.com",
  "password": "azza@12345"
}

id : 72c3cb5d-79f1-44d3-ad48-9dea2c0e1501
```

==Track==

```
name : back end
id : 291fa418-9e0e-4fc5-b370-2c98d1409006
```

---

## Applicants

### /api/Applicants

#### GET

##### Parameters

| Name     | Located in | Description | Required | Schema  |
| -------- | ---------- | ----------- | -------- | ------- |
| page     | query      |             | No       | integer |
| pageSize | query      |             | No       | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Applicants/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id   | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Applicants/{id}/status

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id   | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

---

## Auth

### /api/Auth/register/applicant

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Auth/register/examiner

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Auth/login

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Auth/emailconfirmation

#### GET

##### Parameters

| Name  | Located in | Description | Required | Schema |
| ----- | ---------- | ----------- | -------- | ------ |
| email | query      |             | No       | string |
| token | query      |             | No       | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Auth/forgotpassword

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Auth/resetpassword

#### GET

##### Parameters

| Name  | Located in | Description | Required | Schema |
| ----- | ---------- | ----------- | -------- | ------ |
| email | query      |             | No       | string |
| token | query      |             | No       | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Auth/changepassword

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Auth/updateuseremail

#### PUT

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

---

## Enrollments

### /api/Enrollments

#### GET

##### Parameters

| Name     | Located in | Description | Required | Schema  |
| -------- | ---------- | ----------- | -------- | ------- |
| page     | query      |             | No       | integer |
| pageSize | query      |             | No       | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "data": [
      {
        "id": 1,
        "applicantId": "72c3cb5d-79f1-44d3-ad48-9dea2c0e1501",
        "trackId": 3,
        "trackName": null,
        "enrollmentDate": "2025-04-17T13:00:51.578181",
        "status": "Active"
      },
      {
        "id": 4,
        "applicantId": "72c3cb5d-79f1-44d3-ad48-9dea2c0e1501",
        "trackId": 1,
        "trackName": null,
        "enrollmentDate": "2025-04-17T13:52:17.781583",
        "status": "Active"
      }
    ],
    "page": 1,
    "pageSize": 10,
    "totalCount": 2,
    "totalPages": 1
  }
}
```

### /api/Enrollments/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "id": 1,
    "applicantId": "72c3cb5d-79f1-44d3-ad48-9dea2c0e1501",
    "trackId": 3,
    "trackName": null,
    "enrollmentDate": "2025-04-17T13:00:51.578181",
    "status": "Active"
  }
}
```

### /api/Enrollments/applicant/{applicantId}

#### GET

##### Parameters

| Name        | Located in | Description | Required | Schema  |
| ----------- | ---------- | ----------- | -------- | ------- |
| applicantId | path       |             | Yes      | string  |
| page        | query      |             | No       | integer |
| pageSize    | query      |             | No       | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "data": [
      {
        "id": 1,
        "applicantId": "72c3cb5d-79f1-44d3-ad48-9dea2c0e1501",
        "trackId": 3,
        "trackName": "backend",
        "enrollmentDate": "2025-04-17T13:00:51.578181",
        "status": "Active"
      },
      {
        "id": 4,
        "applicantId": "72c3cb5d-79f1-44d3-ad48-9dea2c0e1501",
        "trackId": 1,
        "trackName": "فرونت اند",
        "enrollmentDate": "2025-04-17T13:52:17.781583",
        "status": "Active"
      }
    ],
    "page": 1,
    "pageSize": 10,
    "totalCount": 2,
    "totalPages": 1
  }
}
```

#### POST

##### Parameters

| Name        | Located in | Description | Required | Schema |
| ----------- | ---------- | ----------- | -------- | ------ |
| applicantId | path       |             | Yes      | string |

##### Body

```json
{
  "trackId": 2
} // int
```

##### Responses

| Code | Description |
| ---- | ----------- |
| 201  | Created     |

```json
{
  "statusCode": "Created",
  "meta": null,
  "succeeded": true,
  "message": "Enrollment created successfully",
  "errors": null,
  "data": {
    "id": 5,
    "applicantId": "72c3cb5d-79f1-44d3-ad48-9dea2c0e1501",
    "trackId": 2,
    "trackName": "فرونت اند",
    "enrollmentDate": "2025-04-17T13:53:20.1266275+03:00",
    "status": "Active"
  }
}
```

### /api/Enrollments/{id}/status

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

###### Body

```json
{
  "status": "Active"
}
```

==Status can be:== Active, Completed, Dropped

==!! may be this EP is not needed==

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Enrollment status updated successfully",
  "errors": null,
  "data": {
    "id": 1,
    "applicantId": "72c3cb5d-79f1-44d3-ad48-9dea2c0e1501",
    "trackId": 3,
    "trackName": null,
    "enrollmentDate": "2025-04-17T13:00:51.578181",
    "status": "Dropped"
  }
}
```

---

## Examiners

### /api/Examiners

#### GET

##### Parameters

| Name     | Located in | Description | Required | Schema  |
| -------- | ---------- | ----------- | -------- | ------- |
| page     | query      |             | No       | integer |
| pageSize | query      |             | No       | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "data": [
      {
        "specialization": "sssssssdddddd",
        "examinerLoads": [
          {
            "id": "1",
            "type": "Exam",
            "maxWorkLoad": 10,
            "currWorkLoad": 0
          },
          {
            "id": "1",
            "type": "Task",
            "maxWorkLoad": 4,
            "currWorkLoad": 1
          }
        ],
        "id": "291fa418-9e0e-4fc5-b370-2c98d1409006",
        "email": "mayarqasarwa@gmail.com",
        "fullName": "اسم جديد",
        "dateOfBirth": "2025-04-08T17:00:29.06",
        "userType": "Examiner",
        "image": null,
        "gender": "Female"
      },
      {
        "specialization": "Front-End",
        "examinerLoads": [],
        "id": "6247d379-064e-4097-a77e-ad3b72a51044",
        "email": "examiner4@exam.test",
        "fullName": "Abrar Arman",
        "dateOfBirth": "2025-04-07T00:00:00",
        "userType": "Examiner",
        "image": "profile-images/f0db583f-4594-4738-ae86-6ab396a8b2ad_لقطة الشاشة 2025-04-12 212319.jpg",
        "gender": null
      }
	]
    "page": 1,
    "pageSize": 10,
    "totalCount": 5,
    "totalPages": 1
  }
}
```

### /api/Examiners/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id   | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "specialization": "Front-End",
    "examinerLoads": [],
    "id": "6247d379-064e-4097-a77e-ad3b72a51044",
    "email": "examiner4@exam.test",
    "fullName": "Abrar Arman",
    "dateOfBirth": "2025-04-07T00:00:00",
    "userType": "Examiner",
    "image": "profile-images/f0db583f-4594-4738-ae86-6ab396a8b2ad_لقطة الشاشة 2025-04-12 212319.jpg",
    "gender": null
  }
}
```

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id   | path       |             | Yes      | string |

###### Body

```json
{
  "fullName": "string",
  "dateOfBirth": "2025-04-17T12:05:15.430Z",
  "gender": "Male",
  "specialization": "string"
}
```

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Examiner updated successfully",
  "errors": null,
  "data": {
    "specialization": "string",
    "examinerLoads": [],
    "id": "6247d379-064e-4097-a77e-ad3b72a51044",
    "email": "examiner4@exam.test",
    "fullName": "string",
    "dateOfBirth": "1987-04-17T12:05:15.43Z",
    "userType": "Examiner",
    "image": "profile-images/f0db583f-4594-4738-ae86-6ab396a8b2ad_لقطة الشاشة 2025-04-12 212319.jpg",
    "gender": "Male"
  }
}
```

### /api/Examiners/{examinerId}/tracks

#### GET ==fetch tracks which are examiner working in==

##### Parameters

| Name       | Located in | Description | Required | Schema  |
| ---------- | ---------- | ----------- | -------- | ------- |
| examinerId | path       |             | Yes      | string  |
| page       | query      |             | No       | integer |
| pageSize   | query      |             | No       | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": 3,
      "seniorExaminerID": "291fa418-9e0e-4fc5-b370-2c98d1409006",
      "name": "backend",
      "description": "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh",
      "objectives": "uuuuuuuuuuuuuuuuuuuuuuuu",
      "associatedSkills": "nnnnnnnnnnnnnnnnnnnnn",
      "isActive": true,
      "image": "string",
      "levels": []
    }
  ]
}
```

### /api/Examiners/{examinerId}/tracks/{trackId}

#### POST

##### Parameters

| Name       | Located in | Description | Required | Schema |
| ---------- | ---------- | ----------- | -------- | ------ |
| examinerId | path       |             | Yes      | string |
| trachId    | path       |             | Yes      | int    |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Track added to examiner",
  "errors": null,
  "data": null
}
```

#### DELETE

##### Parameters

| Name       | Located in | Description | Required | Schema  |
| ---------- | ---------- | ----------- | -------- | ------- |
| examinerId | path       |             | Yes      | string  |
| trackId    | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Track removed from examiner",
  "errors": null,
  "data": null
}
```

### /api/Examiners/{examinerId}/workload

#### GET

##### Parameters

| Name       | Located in | Description | Required | Schema |
| ---------- | ---------- | ----------- | -------- | ------ |
| examinerId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": "1",
      "type": "Exam",
      "maxWorkLoad": 10,
      "currWorkLoad": 0
    },
    {
      "id": "1",
      "type": "Task",
      "maxWorkLoad": 4,
      "currWorkLoad": 1
    }
  ]
}
```

## level-progress

### /api/level-progresses/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "id": 1,
    "enrollmentId": 1,
    "levelId": 5,
    "levelName": null,
    "status": "InProgress",
    "startDate": "2025-04-17T13:00:51.704477",
    "completionDate": "0001-01-01T00:00:00"
  }
}
```

### /api/level-progresses/enrollment/{enrollmentId}

#### GET ==all level progresses for an enrollment (applicant levels)==

##### Parameters

| Name         | Located in | Description | Required | Schema  |
| ------------ | ---------- | ----------- | -------- | ------- |
| enrollmentId | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": 1,
      "enrollmentId": 1,
      "levelId": 5,
      "levelName": "Beginner Level",
      "status": "InProgress",
      "startDate": "2025-04-17T13:00:51.704477",
      "completionDate": "0001-01-01T00:00:00"
    }
  ]
}
```

### /api/level-progresses/enrollment/{enrollmentId}/current

#### GET ==just inProgress level in an enrollment ==

##### Parameters

| Name         | Located in | Description | Required | Schema  |
| ------------ | ---------- | ----------- | -------- | ------- |
| enrollmentId | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "id": 1,
    "enrollmentId": 1,
    "levelId": 5,
    "levelName": "Beginner Level",
    "status": "InProgress", //  <<<<
    "startDate": "2025-04-17T13:00:51.704477",
    "completionDate": "0001-01-01T00:00:00"
  }
}
```

### /api/level-progresses/{id}/status

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

###### Body

```
{
  "status": "string"
}
```

level progress Status: InProgress, Successful, Failed

> > خلي الاشي اوتوماتك

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/level-progresses/applicant/{applicantId}

#### GET ==All levels for applicant >> may be NO NEEDED for it==

##### Parameters

| Name        | Located in | Description | Required | Schema |
| ----------- | ---------- | ----------- | -------- | ------ |
| applicantId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": 1,
      "enrollmentId": 1,
      "levelId": 5,
      "levelName": "Beginner Level",
      "status": "InProgress",
      "startDate": "2025-04-17T13:00:51.704477",
      "completionDate": "0001-01-01T00:00:00"
    },
    {
      "id": 3,
      "enrollmentId": 4,
      "levelId": 3,
      "levelName": "المستوى الأساسي",
      "status": "InProgress",
      "startDate": "2025-04-17T13:52:18.0373336",
      "completionDate": "0001-01-01T00:00:00"
    }
  ]
}
```

### /api/Levels/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### DELETE

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

==change isActive for levels' & stages' ==

### /api/Levels/{id}/stages

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Levels/{levelId}/stages

#### POST

##### Parameters

| Name    | Located in | Description | Required | Schema  |
| ------- | ---------- | ----------- | -------- | ------- |
| levelId | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

## Stage Progresses

### /api/stage-progresses/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "id": 3,
    "levelProgressId": 3,
    "stageId": 1,
    "stageName": null,
    "stageType": "Exam",
    "status": "InProgress",
    "score": 0,
    "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
    "startDate": "2025-04-17T13:52:18.1856041",
    "completionDate": "0001-01-01T00:00:00",
    "attempts": 1
  }
}
```

### /api/stage-progresses/level-progress/{levelProgressId}

#### GET ==return array of stage progresses==

##### Parameters

| Name         | Located in | Description | Required | Schema  |
| ------------ | ---------- | ----------- | -------- | ------- |
| enrollmentId | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": 3,
      "levelProgressId": 3,
      "stageId": 1,
      "stageName": "اختبار تقييمي",
      "stageType": "Exam",
      "status": "InProgress",
      "score": 0,
      "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
      "startDate": "2025-04-17T13:52:18.1856041",
      "completionDate": "0001-01-01T00:00:00",
      "attempts": 1
    }
  ]
}
```

### /api/stage-progresses/level-progress/{levelProgressId}/==current==

#### GET ==return one object==

##### Parameters

| Name         | Located in | Description | Required | Schema  |
| ------------ | ---------- | ----------- | -------- | ------- |
| enrollmentId | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "id": 3,
    "levelProgressId": 4,
    "stageId": 1,
    "stageName": "اختبار تقييمي",
    "stageType": "Exam",
    "status": "InProgress",
    "score": 0,
    "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
    "startDate": "2025-04-17T13:52:18.1856041",
    "completionDate": "0001-01-01T00:00:00",
    "attempts": 1
  }
}
```

### /api/stage-progresses/enrollment/{enrollmentId}/==current==

#### GET ==return one object==

##### Parameters

| Name         | Located in | Description | Required | Schema  |
| ------------ | ---------- | ----------- | -------- | ------- |
| enrollmentId | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "id": 3,
    "levelProgressId": 4,
    "stageId": 1,
    "stageName": "اختبار تقييمي",
    "stageType": "Exam",
    "status": "InProgress",
    "score": 0,
    "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
    "startDate": "2025-04-17T13:52:18.1856041",
    "completionDate": "0001-01-01T00:00:00",
    "attempts": 1
  }
}
```

### /api/stage-progresses/{id}/status

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

###### Body

```Json
{
  "status": "InProgress",
  "score": 0
}
```

ProgressStatus: InProgress, Successful, Failed
==updating based on score // the logic completed 👾==

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Stage progress status updated successfully",
  "errors": null,
  "data": {
    "id": 3,
    "levelProgressId": 3,
    "stageId": 1,
    "stageName": "اختبار تقييمي",
    "stageType": "Exam",
    "status": "Successful",
    "score": 99,
    "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
    "startDate": "2025-04-17T13:52:18.1856041",
    "completionDate": "2025-04-18T13:01:18.9777939+03:00",
    "attempts": 1
  }
}
```

### /api/stage-progresses/applicant/{applicantId}

#### GET ==array of stage progresses==

##### Parameters

| Name        | Located in | Description | Required | Schema |
| ----------- | ---------- | ----------- | -------- | ------ |
| applicantId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": 3,
      "levelProgressId": 3,
      "stageId": 1,
      "stageName": "اختبار تقييمي",
      "stageType": "Exam",
      "status": "Successful",
      "score": 98,
      "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
      "startDate": "2025-04-17T13:52:18.1856041",
      "completionDate": "2025-04-18T13:07:11.7560793",
      "attempts": 0
    },
    {
      "id": 4,
      "levelProgressId": 3,
      "stageId": 2,
      "stageName": "مهمة تطبيقية",
      "stageType": "Task",
      "status": "Failed",
      "score": 6,
      "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
      "startDate": "2025-04-18T13:01:19.4431099",
      "completionDate": "2025-04-18T13:31:34.2188896",
      "attempts": 1
    },
    {
      "id": 10,
      "levelProgressId": 3,
      "stageId": 2,
      "stageName": "مهمة تطبيقية",
      "stageType": "Task",
      "status": "InProgress",
      "score": 0,
      "examinerId": "7d8d8a38-fb80-48f8-a51d-6da2dca5591b",
      "startDate": "2025-04-18T13:52:53.1451343",
      "completionDate": "0001-01-01T00:00:00",
      "attempts": 2
    }
  ]
}
```

### /api/stage-progresses/applicant/{applicantId}/current

#### GET ==current stages in the latest active enrollment ==

##### Parameters

| Name        | Located in | Description | Required | Schema |
| ----------- | ---------- | ----------- | -------- | ------ |
| applicantId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/stage-progresses/applicant/{applicantId}/completed

#### GET ==all completed stages in completed levels for applicant ==

##### Parameters

| Name        | Located in | Description | Required | Schema |
| ----------- | ---------- | ----------- | -------- | ------ |
| applicantId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": 3,
      "levelProgressId": 3,
      "stageId": 1,
      "stageName": "اختبار تقييمي",
      "stageType": "Exam",
      "status": "Successful",
      "score": 98,
      "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
      "startDate": "2025-04-17T13:52:18.1856041",
      "completionDate": "2025-04-18T13:07:11.7560793",
      "attempts": 0
    }
  ]
}
```

### /api/stage-progresses/applicant/{applicantId}/failed

#### GET

##### Parameters

| Name        | Located in | Description | Required | Schema |
| ----------- | ---------- | ----------- | -------- | ------ |
| applicantId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": [
    {
      "id": 4,
      "levelProgressId": 3,
      "stageId": 2,
      "stageName": "مهمة تطبيقية",
      "stageType": "Task",
      "status": "Failed",
      "score": 6,
      "examinerId": "291fa418-9e0e-4fc5-b370-2c98d1409006",
      "startDate": "2025-04-18T13:01:19.4431099",
      "completionDate": "2025-04-18T13:31:34.2188896",
      "attempts": 1
    }
  ]
}
```

### /api/stage-progresses/enrollment/{enrollmentId}/stage/{stageId}/attempt

#### POST ==اذا ما نجح المتقدم في الستيج بقدر يطلب محاولة جديدة الها==

##### Parameters

| Name         | Located in | Description | Required | Schema  |
| ------------ | ---------- | ----------- | -------- | ------- |
| enrollmentId | path       |             | Yes      | integer |
| stageId      | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "Created",
  "meta": null,
  "succeeded": true,
  "message": "New attempt created successfully",
  "errors": null,
  "data": {
    "id": 10,
    "levelProgressId": 3,
    "stageId": 2,
    "stageName": "مهمة تطبيقية",
    "stageType": "Task",
    "status": "InProgress",
    "score": 0,
    "examinerId": "7d8d8a38-fb80-48f8-a51d-6da2dca5591b",
    "startDate": "2025-04-18T13:52:53.1451343+03:00",
    "completionDate": "0001-01-01T00:00:00",
    "attempts": 2
  }
}
```

### /api/stage-progresses/stage/{stageId}/attempts

#### GET ==عدد محاولات الابلكنت لستيج معينة==

##### Parameters

| Name         | Located in | Description | Required | Schema  |
| ------------ | ---------- | ----------- | -------- | ------- |
| enrollmentId | path       |             | Yes      | integer |
| stageId      | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

```json
{
  "statusCode": "OK",
  "meta": null,
  "succeeded": true,
  "message": "Success",
  "errors": null,
  "data": {
    "attemptCount": 1
  }
```

### /api/stage-progresses/{id}/examiner

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Body

```json
{
  "examinerId": "string"
}
```

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

##### Authorization

senior

⛔ ==maybe the senior examiner wants to change the supervisor examiner in stage

---

## Stages

### /api/Stages/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### DELETE

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Stages/{id}/criteria

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### POST

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Tracks/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### DELETE

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Tracks/structure

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Tracks

#### GET

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### PUT

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Tracks/{trackId}/levels

#### POST

##### Parameters

| Name    | Located in | Description | Required | Schema  |
| ------- | ---------- | ----------- | -------- | ------- |
| trackId | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Users/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id   | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### DELETE

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id   | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Users

#### GET

##### Parameters

| Name     | Located in | Description | Required | Schema  |
| -------- | ---------- | ----------- | -------- | ------- |
| userType | query      |             | No       | string  |
| page     | query      |             | No       | integer |
| pageSize | query      |             | No       | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Users/{userId}

#### PUT

##### Parameters

| Name   | Located in | Description | Required | Schema |
| ------ | ---------- | ----------- | -------- | ------ |
| userId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Users/{userId}/profile-image

#### POST

##### Parameters

| Name   | Located in | Description | Required | Schema |
| ------ | ---------- | ----------- | -------- | ------ |
| userId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Workloads/examiner/{examinerId}

#### GET

##### Parameters

| Name       | Located in | Description | Required | Schema |
| ---------- | ---------- | ----------- | -------- | ------ |
| examinerId | path       |             | Yes      | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Workloads/{id}

#### GET

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

#### PUT

##### Parameters

| Name | Located in | Description | Required | Schema  |
| ---- | ---------- | ----------- | -------- | ------- |
| id   | path       |             | Yes      | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

### /api/Workloads

#### POST

##### Responses

| Code | Description |
| ---- | ----------- |
| 200  | Success     |

> > > edit criteria
> > >
> > > -
