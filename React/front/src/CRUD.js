import React, { useState, useEffect, Fragment } from "react";
import Table from 'react-bootstrap/Table';
//import ReactDOM from 'react-dom/client';
import 'bootstrap/dist/css/bootstrap.min.css';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';




const CRUD = () => {


  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);
  // const handleFiles = () => setShow(false);


  const [studentname, setstudentName] = useState('')
  const [studentroll, setstudentRoll] = useState('')
  const [phonenumber, setphoneNumber] = useState('')
  const [studentaddress, setstudentAddress] = useState('')
  const [studentemail, setstudentEmail] = useState('')

  const [editID, setEditId] = useState('');
  const [editstudentName, setEditstudentName] = useState('')
  const [editstudentRoll, setEditstudentRoll] = useState('')
  const [editphoneNumber, setEditphoneNumber] = useState('')
  const [editstudentAddress, setEditstudentAddress] = useState('')
  const [editstudentEmail, setEditstudentEmail] = useState('')




  const [data, setData] = useState([]);

  useEffect(() => {
    getData();
  }, [])


  const getData = () => {
    axios.get('https://localhost:7077/api/Employee')
      .then((result) => {
        setData(result.data)
      })
      .catch((error) => {
        console.log(error)
      })
  }

  const handleEdit = (id) => {
    handleShow();
    axios.get(`https://localhost:7077/api/Employee/${id}`)
      .then((result) => {
        setEditstudentName(result.data.studentName);
        setEditstudentRoll(result.data.studentRoll);
        setEditphoneNumber(result.data.phoneNumber);
        setEditstudentAddress(result.data.studentAddress);
        setEditstudentEmail(result.data.studentEmail);
        setEditId(id);
      })
      .catch((error) => {
        console.log(error)
      })

  }

  const handleDelete = (id) => {
    if (window.confirm("Are you sure to delete this employee") === true) {
      axios.delete(`https://localhost:7077/api/Employee/${id}`)
        .then((result) => {
          if (result.status === 200) {
            toast.success('Student has been deleted');
            getData();
          }
        })
        .catch((error) => {
          toast.error(error);
        })
    }

  }

  const handleUpdate = () => {
    const url = `https://localhost:7077/api/Employee/${editID}`;
    const data = {
      "id": editID,
      "studentName": editstudentName,
      "studentRoll": editstudentRoll,
      "phoneNumber": editphoneNumber,
      "studentAddress": editstudentAddress,
      "studentEmail": editstudentEmail



    }

    axios.put(url, data)
      .then((result) => {
        handleClose();
        getData();
        clear();
        toast.success('Student has been Updated');
      }).catch((error) => {
        toast.error(error);
      })
  }



  const handleSave = () => {
    const url = 'https://localhost:7077/api/Employee';
    const data = {
      "studentName": studentname,
      "studentRoll": studentroll,
      "phoneNumber": phonenumber,
      "studentAddress": studentaddress,
      "studentEmail": studentemail
    }


    axios.post(url, data)
      .then((result) => {
        getData();
        clear();
        toast.success('Student has been Save');
      }).catch((error) => {
        toast.error(error);
      })
  }



  //data clear

  const clear = () => {
    setstudentName('');
    setstudentRoll('');
    setphoneNumber('');
    setstudentAddress('');
    setstudentEmail('')

    setEditstudentName('');
    setEditstudentRoll('');
    setEditphoneNumber('');
    setEditstudentAddress('');
    setEditstudentEmail('');
    setEditId('');

  }
//data clear end

  //Excel

  // const handleClick = () => {
  //   const url = 'https://localhost:7077/api/Employee';
  //   const data = {
  //     "studentName": studentname,
  //     "studentRoll": studentroll,
  //     "phoneNumber": phonenumber,
  //     "studentAddress": studentaddress,
  //     "studentEmail": studentemail
  //   }


  //   axios.post(url, data)
  //     .then((result) => {
  //       getData();
  //       clear();
  //       toast.success('Student has been Save');
  //     }).catch((error) => {
  //       toast.error(error);
  //     })
  // }


  // handleFiles = File => {
  //   console.log(File)
  // }

  // handleFiles = (File) => {
  //   var reader = new FileReader()
  //   reader.readAsText(File[0])
  //   this.setState({ File: files[0] })
  //   this.setState({ UploadFile: true })
  //   this.setState({
  //     FileExtension: files[0].name.split('.').pop(),
  //   })
  // }

  // handleClick = (event) => {
  //   event.preventDefault()
  //   if (this.state.FileExtension !== '') {
  //     const data = new FormData()
  //     data.append('file', this.state.File)
  //     if (this.state.FileExtension.toLowerCase() === 'xlsx') {
  //       service

  //         .then((data) => {
  //           console.log(data)
  //           this.ReadRecord(this.state.PageNumber)
  //         })
  //         .catch((error) => {
  //           console.log(error)
  //           this.ReadRecord(this.state.PageNumber)
  //         })
  //     } else {
  //       console.log('Invalid File')
  //     }
  //   }
  // }

//   const handleFile =(event) =>{
//     const url = 'https://localhost:7077/api/Employee';
//     const file = event.target.files[0];
//     ExcelRenderer(file, (error,response) => {
//       if(error) {
//         console.log(error)
//       } else {
//         setShow(response.rows[0])
//         setData(response.rows)
//       }

//     })
//   }

  
//   axios.post(url, data)
//   .then((result) => {
//     getData();
//     clear();
//     toast.success('Student has been Save');
//   }).catch((error) => {
//     toast.error(error);
//   })
// }

  // End Excel

  return (
    <Fragment>
      <ToastContainer />

      <div className="Container">

        <div className="row">
          <div className="col-3">

            <h3 className="Insert">Insert Student</h3>

            <Col>
              <input type="text" className="form-control" placeholder="StudentName"
                value={studentname} onChange={(e) => setstudentName(e.target.value)} required

              />
            </Col> <br></br>


            <Col>
              <input type="text" className="form-control" placeholder="StudentRoll"
                value={studentroll} onChange={(e) => setstudentRoll(e.target.value)} required
              />
            </Col> <br></br>

            <Col>
              <input type="text" className="form-control" placeholder="PhoneNumber"
                value={phonenumber} onChange={(e) => setphoneNumber(e.target.value)} required
              />
            </Col> <br></br>

            <Col>
              <input type="text" className="form-control" placeholder="StudentEmail"
                value={studentemail} onChange={(e) => setstudentEmail(e.target.value)} required
              />
            </Col> <br></br>

            <Col>
              <input type="text" className="form-control" placeholder="StudentAddress"
                value={studentaddress} onChange={(e) => setstudentAddress(e.target.value)} required
              />
            </Col> <br></br>



            <Col>
              <button className="btn btn-outline-primary" onClick={() => handleSave()}>Submit</button>
            </Col> <br></br>

          </div>

          <div className="col-9 ">


            <h3 className="List">Student List</h3>
            <Table striped bordered hover>


              <thead>
                <tr>
                  <th>Index No</th>
                  <th>ID</th>
                  <th>studentName</th>
                  <th>studentRoll</th>
                  <th>phoneNumber</th>
                  <th>studentEmail</th>
                  <th>studentAddress</th>
                  <th>Action</th>

                </tr>
              </thead>
              <tbody>
                {
                  data && data.length > 0 ?
                    data.map((item, index) => {
                      return (

                        <tr key={index}>
                          <td>{index + 1}</td>
                          <td>{item.id}</td>
                          <td>{item.studentName}</td>
                          <td>{item.studentRoll}</td>
                          <td>{item.phoneNumber}</td>
                          <td>{item.studentEmail}</td>
                          <td>{item.studentAddress}</td>

                          <td colSpan={2}>
                            <button className="btn btn-primary" onClick={() => handleEdit(item.id)}>Edit</button> &nbsp;
                            <button className="btn btn-danger" onClick={() => handleDelete(item.id)}>Delete</button>
                          </td>

                        </tr>


                      )
                    })
                    :
                    'Loading...'
                }


              </tbody>
            </Table>

          </div>
        </div>
      </div>
      <br></br>




      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Modify / Update Student</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Row>
            <Col>
              <input type="text" className="form-control" placeholder="Name"
                value={editstudentName} onChange={(e) => setEditstudentName(e.target.value)}
              />
            </Col>

            <Col>
              <input type="text" className="form-control" placeholder="Roll"
                value={editstudentRoll} onChange={(e) => setEditstudentRoll(e.target.value)}
              />
            </Col>

            <Col>
              <input type="text" className="form-control" placeholder="Phone"
                value={editphoneNumber} onChange={(e) => setEditphoneNumber(e.target.value)}
              />
            </Col>

            <Col>
              <input type="text" className="form-control" placeholder="Email"
                value={editstudentEmail} onChange={(e) => setEditstudentEmail(e.target.value)}
              />
            </Col>

            <Col>
              <input type="text" className="form-control" placeholder="Address"
                value={editstudentAddress} onChange={(e) => setEditstudentAddress(e.target.value)}
              />
            </Col>



          </Row>

        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={handleUpdate}>
            Save Changes
          </Button>
        </Modal.Footer>
      </Modal>

    </Fragment>

  )
}

export default CRUD;