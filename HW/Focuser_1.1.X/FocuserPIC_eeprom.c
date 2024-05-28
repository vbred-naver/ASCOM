/* FocuserPIC_eeprom.c" */
#ifdef _FOCUSER_

/* revision */
bool eeprom_save_revision(unsigned char revision)
{
	if(i2c1_write_byte(ADDR_REVISION, revision) == false) return false;
	return true;
}

/* current step */
bool eeprom_save_step(signed long int c_posi)
{
	eep_write_buffer[0] = (unsigned char)(c_posi);
	eep_write_buffer[1] = (unsigned char)(c_posi >> 8);
	eep_write_buffer[2] = (unsigned char)(c_posi >> 16);
	eep_write_buffer[3] = (unsigned char)(c_posi >> 24);
	if(i2c1_write_page(ADDR_POSI, 4) == false) return false;
	return true;
}

/* all read */
bool eeprom_data_read_all(void)
{
	unsigned char write_err_count = 0;
	bool result;
	
	if(i2c1_read_sequential(0, 10) == 1)
	{
		/* revision */
		revision = eep_read_buffer[ADDR_REVISION];
		if(revision > 100){
			revision = 0;
			if(eeprom_save_revision(revision) == false) ++write_err_count;
		}

		/* current step */
		c_posi = make32(eep_read_buffer[ADDR_POSI + 3],eep_read_buffer[ADDR_POSI + 2],eep_read_buffer[ADDR_POSI + 1],eep_read_buffer[ADDR_POSI]);
		
		if(write_err_count == 0) result = true;
	}
	else
	{
		result = false;
	}
	return result;
}

/* default default save */
bool eeprom_default_save(void){			  // 24c02 write
	unsigned char write_err_count = 0;

	// revision
	revision = 0;
	if(eeprom_save_revision(revision) == 0) ++write_err_count;

	// current step
	c_posi = 0;
	if(eeprom_save_step(c_posi) == 0) ++write_err_count;

	if(write_err_count == 0) return true;
	return false;
}

//************************************************************************************//
// eeprom Initialize & first read
//************************************************************************************//
void init_eeprom(void)
{
	// I2C
	__delay_ms(1000);
	i2c1_initial();
	__delay_us(1000);
	
	eep_status = NOALARM;
	if(eeprom_data_read_all() == false)
	{
		i2c1_initial();						// retry
		__delay_us(1000);
		if(eeprom_data_read_all() == false)
		{
			i2c1_initial();
			__delay_us(1000);
			if(eeprom_data_read_all() == false)
			{
				eep_status = ALARM;      // eeprom fault=1
			}
		}
	}
}

#endif