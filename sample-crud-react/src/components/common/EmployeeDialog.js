import React, { useEffect, useState } from 'react';
import {
  Dialog, DialogTitle, DialogContent, DialogActions,
  Button, FormControl, FormLabel, RadioGroup,
  FormControlLabel, Radio, MenuItem, Typography, TextField
} from '@mui/material';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';

import ReusableTextField from './ReusableTextField';
import { getCafes, saveEmployee } from '../../services/api';
import {validateEmail, validatePhone } from '../../utils/validation';
import dayjs from 'dayjs';
const defaultForm = {
  id: '',
  name: '',
  emailAddress: '',
  phoneNumber: '',
  gender: '',
  cafeId: '',
  employmentDate: null
};

function EmployeeDialog({ open, onClose, formData = {}, onSave }) {
  const [form, setForm] = useState({ ...defaultForm });
  const [errors, setErrors] = useState({});
  const [cafes, setCafes] = useState([]);

  useEffect(() => {
    getCafes().then((res) => setCafes(res.data)).catch(() => setCafes([]));
    const preparedForm = {
      ...defaultForm,
      ...formData,
      employmentDate: (formData!==null && formData.employmentDate) ? dayjs(formData.employmentDate) : null
    };
    setForm(preparedForm);
   
     const newErrors = {};
    setErrors(newErrors);
  }, [formData]);

  const handleOnChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
    if (value?.trim()) {
      setErrors((prev) => ({ ...prev, [name]: '' }));
    }
  };
 const handleDateChange = (newDate) => {
    setForm((prev) => ({ ...prev, employmentDate: newDate }));
  };
  const validateForm = () => {
    const newErrors = {};
    if (!form.name?.trim()) newErrors.name = 'Please enter name';
    if (!form.emailAddress?.trim()) newErrors.emailAddress = 'Please enter email';
    if (!form.phoneNumber?.trim()) newErrors.phoneNumber = 'Please enter phone number';
    if (!form.gender?.trim()) newErrors.gender = 'Please select gender';
    if (!validateEmail(form.emailAddress)) newErrors.emailAddress = 'Please enter valid email';
    if (!validatePhone(form.phoneNumber)) newErrors.phoneNumber = 'Phone number must start with 8 or 9 and contain 8 digits';
    // if (form.cafeId && (!form.employmentDate || !dayjs(form.employmentDate).isValid())) {
    //   newErrors.employmentDate = 'Please select employment start date';
    // }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validateForm()) return;
    
       const inputForm = {
        name: form.name?.trim() || '',
        emailAddress: form.emailAddress?.trim() || '',
        phoneNumber: form.phoneNumber?.trim() || '',
        gender: form.gender?.trim() || '',
        cafeId:form.cafeId?.trim() || '',
        employmentDate:form.employmentDate!==null && form.employmentDate.isValid() ? form.employmentDate.toISOString() : null,
        ...(form.id && { id: form.id })
    };

    try {          
      const res = await saveEmployee(inputForm);
      if (res?.data.status==='success') {        
        onSave?.();
        onClose();
      }else {
         alert(res?.data.message);
      }
    } catch (error) {
      console.error(error);
      alert('An error occurred while saving the employee.');
    }
  };

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>{form.id ? 'Edit' : 'Add'} Employee</DialogTitle>
       
      <DialogContent dividers>
        <ReusableTextField
          name="name"
          label="Name"
          value={form.name}
          onChange={handleOnChange}
          error={errors.name}
          required
        />
        <ReusableTextField
          name="emailAddress"
          label="Email"
          value={form.emailAddress}
          onChange={handleOnChange}
          error={errors.emailAddress}
          required
        />
        <ReusableTextField
          name="phoneNumber"
          label="Phone"
          value={form.phoneNumber}
          onChange={handleOnChange}
          error={errors.phoneNumber}
          required
        />

        <FormControl component="fieldset" margin="normal" error={!!errors.gender} required>
          <FormLabel component="legend">Gender</FormLabel>
          <RadioGroup
            row
            name="gender"
            value={form.gender}
            onChange={handleOnChange}
          >
            <FormControlLabel value="Male" control={<Radio />} label="Male" />
            <FormControlLabel value="Female" control={<Radio />} label="Female" />
          </RadioGroup>
          {errors.gender && (
            <Typography variant="caption" color="error">
              {errors.gender}
            </Typography>
          )}
        </FormControl>

        <TextField
          select
          name="cafeId"
          label="Assigned Cafe"
          value={form.cafeId}
          onChange={handleOnChange}
          fullWidth
          margin="normal"
        >
          {cafes.map((cafe) => (
            <MenuItem key={cafe.id} value={cafe.id}>
              {cafe.name}
            </MenuItem>
          ))}
        </TextField>
        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <DemoContainer components={['DatePicker']}>
            <DatePicker
              label="Employment Start Date"
              value={form.employmentDate ?? null}
              onChange={handleDateChange}
              disabled={!form.cafeId}
              format="DD/MM/YYYY"             
            />
          </DemoContainer>
        </LocalizationProvider>
      </DialogContent>
      <DialogActions>       
        <Button variant="contained" onClick={handleSubmit}>{form.id ? 'Update' : 'Save'}</Button>
         <Button  onClick={onClose}>Cancel</Button>
      </DialogActions>
    </Dialog>
  );
}

export default EmployeeDialog;

