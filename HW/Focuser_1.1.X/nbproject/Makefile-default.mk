#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Include project Makefile
ifeq "${IGNORE_LOCAL}" "TRUE"
# do not include local makefile. User is passing all local related variables already
else
include Makefile
# Include makefile containing local settings
ifeq "$(wildcard nbproject/Makefile-local-default.mk)" "nbproject/Makefile-local-default.mk"
include nbproject/Makefile-local-default.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=default
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
IMAGE_TYPE=debug
OUTPUT_SUFFIX=elf
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
else
IMAGE_TYPE=production
OUTPUT_SUFFIX=hex
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
endif

ifeq ($(COMPARE_BUILD), true)
COMPARISON_BUILD=-mafrlcsj
else
COMPARISON_BUILD=
endif

# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}

# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Source Files Quoted if spaced
SOURCEFILES_QUOTED_IF_SPACED=FocuserPIC.c FocuserPIC_motor.c FocuserPIC_ds1820.c FocuserPIC_uart.c FocuserPIC_24C02.c FocuserPIC_eeprom.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/FocuserPIC.o ${OBJECTDIR}/FocuserPIC_motor.o ${OBJECTDIR}/FocuserPIC_ds1820.o ${OBJECTDIR}/FocuserPIC_uart.o ${OBJECTDIR}/FocuserPIC_24C02.o ${OBJECTDIR}/FocuserPIC_eeprom.o
POSSIBLE_DEPFILES=${OBJECTDIR}/FocuserPIC.o.d ${OBJECTDIR}/FocuserPIC_motor.o.d ${OBJECTDIR}/FocuserPIC_ds1820.o.d ${OBJECTDIR}/FocuserPIC_uart.o.d ${OBJECTDIR}/FocuserPIC_24C02.o.d ${OBJECTDIR}/FocuserPIC_eeprom.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/FocuserPIC.o ${OBJECTDIR}/FocuserPIC_motor.o ${OBJECTDIR}/FocuserPIC_ds1820.o ${OBJECTDIR}/FocuserPIC_uart.o ${OBJECTDIR}/FocuserPIC_24C02.o ${OBJECTDIR}/FocuserPIC_eeprom.o

# Source Files
SOURCEFILES=FocuserPIC.c FocuserPIC_motor.c FocuserPIC_ds1820.c FocuserPIC_uart.c FocuserPIC_24C02.c FocuserPIC_eeprom.c



CFLAGS=
ASFLAGS=
LDLIBSOPTIONS=

############# Tool locations ##########################################
# If you copy a project from one host to another, the path where the  #
# compiler is installed may be different.                             #
# If you open this project with MPLAB X in the new host, this         #
# makefile will be regenerated and the paths will be corrected.       #
#######################################################################
# fixDeps replaces a bunch of sed/cat/printf statements that slow down the build
FIXDEPS=fixDeps

.build-conf:  ${BUILD_SUBPROJECTS}
ifneq ($(INFORMATION_MESSAGE), )
	@echo $(INFORMATION_MESSAGE)
endif
	${MAKE}  -f nbproject/Makefile-default.mk ${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=33FJ12MC201
MP_LINKER_FILE_OPTION=,--script=p33FJ12MC201.gld
# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/FocuserPIC.o: FocuserPIC.c  .generated_files/flags/default/4a1f7d940414fd8ae51ee48e98d4bbb8e0a56d11 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC.c  -o ${OBJECTDIR}/FocuserPIC.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC.o.d"      -g -D__DEBUG     -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_motor.o: FocuserPIC_motor.c  .generated_files/flags/default/34cff3676505472f17490d06dd696396cdf7861c .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_motor.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_motor.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_motor.c  -o ${OBJECTDIR}/FocuserPIC_motor.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_motor.o.d"      -g -D__DEBUG     -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_ds1820.o: FocuserPIC_ds1820.c  .generated_files/flags/default/916cd6861c676b67e15ca514e5bd7a3ad98308bb .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_ds1820.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_ds1820.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_ds1820.c  -o ${OBJECTDIR}/FocuserPIC_ds1820.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_ds1820.o.d"      -g -D__DEBUG     -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_uart.o: FocuserPIC_uart.c  .generated_files/flags/default/71d02213df210562fd02f26426ca697f5f863772 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_uart.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_uart.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_uart.c  -o ${OBJECTDIR}/FocuserPIC_uart.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_uart.o.d"      -g -D__DEBUG     -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_24C02.o: FocuserPIC_24C02.c  .generated_files/flags/default/7e9d8fa2f21972254b23a9d5820ec607464b0677 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_24C02.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_24C02.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_24C02.c  -o ${OBJECTDIR}/FocuserPIC_24C02.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_24C02.o.d"      -g -D__DEBUG     -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_eeprom.o: FocuserPIC_eeprom.c  .generated_files/flags/default/1f0022bd9986eb7c93c6631903c9a96789b94cf9 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_eeprom.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_eeprom.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_eeprom.c  -o ${OBJECTDIR}/FocuserPIC_eeprom.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_eeprom.o.d"      -g -D__DEBUG     -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
else
${OBJECTDIR}/FocuserPIC.o: FocuserPIC.c  .generated_files/flags/default/4c5b7c3cb8861e0650752c3f1df7d2c3438d46a3 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC.c  -o ${OBJECTDIR}/FocuserPIC.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC.o.d"        -g -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_motor.o: FocuserPIC_motor.c  .generated_files/flags/default/8c9c833ee3ae7362755bbf5e0d61322fa1f031bf .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_motor.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_motor.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_motor.c  -o ${OBJECTDIR}/FocuserPIC_motor.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_motor.o.d"        -g -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_ds1820.o: FocuserPIC_ds1820.c  .generated_files/flags/default/f533f3c76109903c238b6983001811352496eedc .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_ds1820.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_ds1820.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_ds1820.c  -o ${OBJECTDIR}/FocuserPIC_ds1820.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_ds1820.o.d"        -g -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_uart.o: FocuserPIC_uart.c  .generated_files/flags/default/4dd1d08c3ad8232c6ff65612f4fa02685f53e412 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_uart.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_uart.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_uart.c  -o ${OBJECTDIR}/FocuserPIC_uart.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_uart.o.d"        -g -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_24C02.o: FocuserPIC_24C02.c  .generated_files/flags/default/dffcfaa88f213cddff86157a0d54d04fa5552545 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_24C02.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_24C02.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_24C02.c  -o ${OBJECTDIR}/FocuserPIC_24C02.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_24C02.o.d"        -g -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
${OBJECTDIR}/FocuserPIC_eeprom.o: FocuserPIC_eeprom.c  .generated_files/flags/default/1ed3068cbcf5633d7bc337601e46c7de946fe960 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/FocuserPIC_eeprom.o.d 
	@${RM} ${OBJECTDIR}/FocuserPIC_eeprom.o 
	${MP_CC} $(MP_EXTRA_CC_PRE)  FocuserPIC_eeprom.c  -o ${OBJECTDIR}/FocuserPIC_eeprom.o  -c -mcpu=$(MP_PROCESSOR_OPTION)  -MP -MMD -MF "${OBJECTDIR}/FocuserPIC_eeprom.o.d"        -g -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -msmall-code -msmall-data -O0 -msmart-io=1 -Wall -msfr-warn=off    -mdfp="${DFP_DIR}/xc16"
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assemblePreproc
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE)  -o ${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -mcpu=$(MP_PROCESSOR_OPTION)        -D__DEBUG=__DEBUG   -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)   -mreserve=data@0x800:0x81F -mreserve=data@0x820:0x821 -mreserve=data@0x822:0x823 -mreserve=data@0x824:0x825 -mreserve=data@0x826:0x84F   -Wl,,,--defsym=__MPLAB_BUILD=1,--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1,-D__DEBUG=__DEBUG,,$(MP_LINKER_FILE_OPTION),--stack=16,--check-sections,--data-init,--pack-data,--handles,--isr,--no-gc-sections,--fill-upper=0,--stackguard=16,--no-force-link,--smart-io,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--report-mem,--memorysummary,${DISTDIR}/memoryfile.xml$(MP_EXTRA_LD_POST)  -mdfp="${DFP_DIR}/xc16" 
	
else
${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE)  -o ${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -mcpu=$(MP_PROCESSOR_OPTION)        -omf=elf -DXPRJ_default=$(CND_CONF)    $(COMPARISON_BUILD)  -Wl,,,--defsym=__MPLAB_BUILD=1,$(MP_LINKER_FILE_OPTION),--stack=16,--check-sections,--data-init,--pack-data,--handles,--isr,--no-gc-sections,--fill-upper=0,--stackguard=16,--no-force-link,--smart-io,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--report-mem,--memorysummary,${DISTDIR}/memoryfile.xml$(MP_EXTRA_LD_POST)  -mdfp="${DFP_DIR}/xc16" 
	${MP_CC_DIR}\\xc16-bin2hex ${DISTDIR}/Focuser_1.1.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} -a  -omf=elf   -mdfp="${DFP_DIR}/xc16" 
	
endif


# Subprojects
.build-subprojects:


# Subprojects
.clean-subprojects:

# Clean Targets
.clean-conf: ${CLEAN_SUBPROJECTS}
	${RM} -r ${OBJECTDIR}
	${RM} -r ${DISTDIR}

# Enable dependency checking
.dep.inc: .depcheck-impl

DEPFILES=$(shell mplabwildcard ${POSSIBLE_DEPFILES})
ifneq (${DEPFILES},)
include ${DEPFILES}
endif
