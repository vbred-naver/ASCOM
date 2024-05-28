/* FocuserPIC_motor.c" */
#ifdef _FOCUSER_

//************************************************************************************//
// motor Initialize
// TB6608FNG
//************************************************************************************//
void init_motor(void)
{
	MOTOR_CLK = 0;		// Motor Clock(output)
	MOTOR_DIR = 0;		// Motor Direction(output), L:CW
	MOTOR_EN = 0;		// Motor Enable(output), L:Disable
	MOTOR_RST = 0;		// Motor Reset(output), L:Reset
	__delay_us(5);
	MOTOR_RST = 1;		// Motor Reset(output), L:Reset
}

//************************************************************************************//
// Motor On
//************************************************************************************//
void motor_on(void)
{
	MOTOR_EN = 1;		// Motor Enable(output), L:Disable
}

//************************************************************************************//
// Motor Off
//************************************************************************************//
void motor_off(void)
{
	MOTOR_EN = 0;		// Motor Enable(output), L:Disable
}

//************************************************************************************//
// step driver
// TB6608FNG
//************************************************************************************//
void step_driver(void)
{
	if (absolute)	// plus only, MO200#, MO0#
	{
		if(r_posi > c_posi)							// �̵��ؾ� �� ���� ���簪���� Ŭ��
		{
			if (max_step > c_posi)					// ���밪�� max_step ������ ũ�� �ȵ�
			{
				MOTOR_DIR = 0;
				if(swing == 0)						// odd, �޽�����
				{
					swing = 1;
					MOTOR_CLK = 1;
				}
				else
				{
					swing = 0;
					MOTOR_CLK = 0;
					++c_posi;						// ����
					if (halt)
					{
						halt = 0;
						r_posi = c_posi;			// ������ ����
					}
				}
			}
			else
			{
				r_posi = c_posi;					// �Ϸ�
			}
		}
		else if(r_posi < c_posi)
		{
			if (c_posi > 0)							// ���밪�� 0 ���� ������ �ȵ�
			{
				MOTOR_DIR = 1;
				if(swing == 0)		// odd
				{
					swing = 1;
					MOTOR_CLK = 1;
				}
				else
				{
					swing = 0;
					MOTOR_CLK = 0;
					--c_posi;
					if (halt)
					{
						halt = 0;
						r_posi = c_posi;
					}
				}
			}
			else
			{
				r_posi = c_posi;
			}
		}
		else
		{
			stability = 1;							// ����ȭ ����
			motor_start = 0;						// step_driver ���� ����
			eeprom_save_step(c_posi);				// ���ܰ� ����
		}
	}
	else		// relative, MO200#, MO-200#
	{
		if(r_posi > 0)								// ��밪�� ���� ���Ұ� ����, ��ġ���� ����
		{
			MOTOR_DIR = 0;
			if(swing == 0)		// odd
			{
				swing = 1;
				MOTOR_CLK = 1;
			}
			else
			{
				swing = 0;
				MOTOR_CLK = 0;
				--r_posi;
				if (halt)
				{
					halt = 0;
					r_posi = 0;
				}
			}
		}
		else if(r_posi < 0)
		{
			MOTOR_DIR = 1;
			if(swing == 0)		// odd
			{
				swing = 1;
				MOTOR_CLK = 1;
			}
			else
			{
				swing = 0;
				MOTOR_CLK = 0;
				++r_posi;
				if (halt)
				{
					halt = 0;
					r_posi = 0;
				}
			}
		}
		else
		{
			stability = 1;
			motor_start = 0;
		}
	}
}

#endif
